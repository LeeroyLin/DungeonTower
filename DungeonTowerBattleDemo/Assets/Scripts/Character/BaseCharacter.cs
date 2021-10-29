using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter
{
    #region 公共字段
    public bool isMember;
    public string name;
    public int rowIdx;
    public int colIdx;
    public bool isMoving;
    public Transform node;
    public float moveSpeed;
    #endregion

    #region 私有字段
    List<Vector2Int> _listPath;
    int _nextPathIdx;
    #endregion

    #region 构造方法
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="name"></param>
    public BaseCharacter(string name)
    {
        this.name = name;
        moveSpeed = CharacterCtrller.Ins.moveSpeed;
    }
    #endregion

    #region 公共方法
    /// <summary>
    /// 设置位置下标
    /// </summary>
    public void SetPosIdx(int rowIdx, int colIdx)
    {
        this.rowIdx = rowIdx;
        this.colIdx = colIdx;
    }

    /// <summary>
    /// 开始移动
    /// </summary>
    public void StartMove(List<Vector2Int> listPath)
    {
        isMoving = true;
        _nextPathIdx = 1;
        _listPath = listPath;
    }

    /// <summary>
    /// 执行移动
    /// </summary>
    public void DoMove()
    {
        // 是否没有移动
        if (!isMoving)
        {
            return;
        }

        // 获取目标位置
        Vector2Int posIdx = _listPath[_nextPathIdx];
        Vector3 targetPos = MapMgr.Ins.GetPosByIdx(posIdx.x, posIdx.y, isMember);
        targetPos.y = node.position.y;

        // 是否没有到目的地
        if (node.position != targetPos)
        {
            // 获取距离
            float dis = Vector3.Distance(targetPos, node.position);
            float moveDis = moveSpeed * Time.deltaTime;

            // 是否移动距离大于目标距离
            if (moveDis >= dis)
            {
                // 直接设置到目标位置
                node.position = targetPos;

                // 还有下一个目标位置
                if (_nextPathIdx < _listPath.Count - 1)
                {
                    // 下移目标
                    _nextPathIdx++;
                }
                else
                {
                    // 标记
                    isMoving = false;
                }
            }
            else
            {
                // 移动
                node.position += moveDis * (targetPos - node.position).normalized;
            }
        }
    }
    #endregion
}
