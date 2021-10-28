using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseCharacter
{
    #region 构造方法
    /// <summary>
    /// 构造方法
    /// </summary>
    public Enemy(string name) : base(name)
    {
        isMember = false;
    }
    #endregion
}
