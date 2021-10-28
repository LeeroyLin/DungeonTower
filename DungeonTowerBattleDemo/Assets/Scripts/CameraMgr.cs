using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : SingletonScript<CameraMgr>
{
    #region 公共字段
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed = 1;
    #endregion

    #region 私有字段
    Camera _camera;
    Vector3 _lastPos;
    Vector3 _moveDir; 
    #endregion

    #region 默认回调
    /// <summary>
    /// 唤醒后回调
    /// </summary>
    private void Awake()
    {
        // 获得组件
        _camera = GetComponent<Camera>();
    }

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

        // 执行移动
        DoMove();
    }
    #endregion

    #region 其他方法
    /// <summary>
    /// 检测输入
    /// </summary>
    void CheckInput()
    {
        _moveDir = Vector3.zero;

        // 鼠标中间按下
        if (Input.GetMouseButtonDown(2))
        {
            // 记录位置
            _lastPos = Input.mousePosition;
        }
        // 鼠标中间拖动
        else if (Input.GetMouseButton(2))
        {
            // 设置移动
            _moveDir.x = Input.mousePosition.x - _lastPos.x;
            _moveDir.z = Input.mousePosition.y - _lastPos.y;
            _moveDir = -_moveDir;

            // 记录位置
            _lastPos = Input.mousePosition;
        }
    }

    /// <summary>
    /// 执行移动
    /// </summary>
    void DoMove()
    {
        transform.Translate(_moveDir * moveSpeed * Time.deltaTime, Space.World);
    }
    #endregion
}
