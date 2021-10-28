using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter
{
    public bool isMember;
    public string name;
    public int rowIdx;
    public int colIdx;

    #region 构造方法
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="name"></param>
    public BaseCharacter(string name)
    {
        this.name = name;
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
    #endregion
}
