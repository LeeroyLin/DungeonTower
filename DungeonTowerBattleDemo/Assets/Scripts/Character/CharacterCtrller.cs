using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrller : SingletonScript<CharacterCtrller>
{
    #region 公共字段
    public LayerMask layers;
    public LineRenderer line;
    public float moveSpeed = 1;
    #endregion

    #region 私有字段
    RaycastHit _hit;

    /// <summary>
    /// 开始的下标
    /// </summary>
    Vector2Int _startPosIdx;

    /// <summary>
    /// 当前下标
    /// </summary>
    Vector2Int _currPosIdx;

    /// <summary>
    /// 目标下标
    /// </summary>
    Vector2Int _targetPosIdx;

    /// <summary>
    /// 是否拖拽预览路径
    /// </summary>
    bool _isDragPath;

    /// <summary>
    /// 路径
    /// </summary>
    List<Vector2Int> _listPath;
    #endregion

    #region 默认回调
    /// <summary>
    /// 开始后回调
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// 每帧回调
    /// </summary>
    void Update()
    {
        // 检测输入
        CheckInput();
    }
    #endregion

    #region 公共方法
    #endregion

    #region 其他方法
    /// <summary>
    /// 检测输入
    /// </summary>
    void CheckInput()
    {
        // 按下鼠标右键
        if (Input.GetMouseButtonDown(1))
        {
            // 获得当前鼠标位置对应的下标
            if (GetMousePosIdx(out _startPosIdx, MapMgr.Ins.planes[0]))
            {
                // 是否该位置有角色可移动
                if (CheckMemberCouldMove(_startPosIdx))
                {
                    // 标记
                    _isDragPath = true;
                }
            }
        }
        // 拖拽鼠标右键
        else if (Input.GetMouseButton(1))
        {
            // 是否没预览
            if (!_isDragPath)
            {
                return;
            }

            // 获得当前鼠标位置对应的下标
            if (GetMousePosIdx(out _currPosIdx, MapMgr.Ins.planes[0]))
            {
                // 是否与目标相同
                if (_currPosIdx == _targetPosIdx)
                {
                    return;
                }
                _targetPosIdx = _currPosIdx;

                // 是否目标与开始下标不同
                if (_startPosIdx != _targetPosIdx)
                {
                    // 获取该位置数据
                    MapTileData tileData = MapMgr.Ins.mapMember[_targetPosIdx.x, _targetPosIdx.y];

                    // 是否有障碍物
                    if (tileData.obstacle > 0)
                    {
                        // 隐藏路径
                        line.gameObject.SetActive(false);

                        return;
                    }

                    // 尝试显示路径
                    TryShowPath();
                }
                else
                {
                    // 相同则隐藏路径
                    line.gameObject.SetActive(false);
                }
            }
            else
            {
                // 重置
                _targetPosIdx = Vector2Int.one * -1000;

                // 隐藏路径
                line.gameObject.SetActive(false);
            }
        }
        // 松开鼠标右键
        else if (Input.GetMouseButtonUp(1))
        {
            // 是否没预览
            if (!_isDragPath)
            {
                return;
            }

            // 是否显示了预览路径
            if (line.gameObject.activeSelf)
            {
                // 开始移动
                StartMove();
            }

            // 重置
            _targetPosIdx = Vector2Int.one * -1000;

            // 隐藏路径
            line.gameObject.SetActive(false);

            // 标记
            _isDragPath = false;
        }
    }

    /// <summary>
    /// 获得当前鼠标位置对应的下标
    /// </summary>
    /// <returns></returns>
    bool GetMousePosIdx(out Vector2Int pos, Transform targetPlane = default)
    {
        pos = Vector2Int.zero;

        // 鼠标位置射线检测
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit, float.MaxValue, layers.value))
        {
            // 平台不符合
            if (targetPlane != default && targetPlane != _hit.transform)
            {
                return false;
            }

            // 获取位置下标
            float leftPos = -MapMgr.Ins.width * 0.5f * MapMgr.Ins.perSize;
            int colIdx = Mathf.FloorToInt(_hit.point.x - leftPos) / MapMgr.Ins.perSize;
            pos.y = Mathf.Clamp(colIdx, 0, MapMgr.Ins.width - 1);

            float bottomPos = _hit.transform.position.z - MapMgr.Ins.halfHeight * 0.5f * MapMgr.Ins.perSize;
            int rowIdx = Mathf.FloorToInt(_hit.point.z - bottomPos) / MapMgr.Ins.perSize;
            pos.x = Mathf.Clamp(rowIdx, 0, MapMgr.Ins.halfHeight - 1);

            return true;
        }

        return false;
    }

    /// <summary>
    /// 尝试显示路径
    /// </summary>
    void TryShowPath()
    {
        // 线高
        float y = 0.2f;

        // 获得路径
        _listPath = MapMgr.Ins.GetPath(_startPosIdx, _targetPosIdx, true);

        // 设置线段点
        List<Vector3> listPos = new List<Vector3>();
        foreach (var item in _listPath)
        {
            listPos.Add(MapMgr.Ins.GetPosByIdx(item.x, item.y, true, y));
        }
        line.positionCount = listPos.Count;
        line.SetPositions(listPos.ToArray());

        // 显示路径
        line.gameObject.SetActive(true);
    }

    /// <summary>
    /// 检测成员是否可移动
    /// </summary>
    /// <param name="posIdx"></param>
    /// <returns></returns>
    bool CheckMemberCouldMove(Vector2Int posIdx)
    {
        // 获取该位置数据
        MapTileData tileData = MapMgr.Ins.mapMember[posIdx.x, posIdx.y];

        // 是否有角色 且 没移动
        return tileData.character != null && !tileData.character.isMoving;
    }

    /// <summary>
    /// 开始移动
    /// </summary>
    void StartMove()
    {
        // 获取位置数据
        MapTileData startTileData = MapMgr.Ins.mapMember[_startPosIdx.x, _startPosIdx.y];
        MapTileData targetTileData = MapMgr.Ins.mapMember[_targetPosIdx.x, _targetPosIdx.y];

        // 设置新位置
        targetTileData.character = startTileData.character;
        targetTileData.character.SetPosIdx(targetTileData.rowIdx, targetTileData.colIdx);
        targetTileData.character.StartMove(_listPath);

        // 置空原位置
        startTileData.character = null;
    }
    #endregion
}
