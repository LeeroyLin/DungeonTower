using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : BaseCharacter
{
    #region 构造方法
    /// <summary>
    /// 构造方法
    /// </summary>
    public Member(string name) : base(name)
    {
        isMember = true;
    }
    #endregion
}
