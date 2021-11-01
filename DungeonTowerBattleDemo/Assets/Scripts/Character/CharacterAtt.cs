using UnityEngine;
using System.Collections;
using System;

public class CharacterAtt
{
    #region 公共字段
    public Att Hp { get; private set; }
    public Att Mp { get; private set; }
    public Att Vit { get; private set; }
    #endregion

    #region 构造方法
    public CharacterAtt(int maxHp, int maxMp, int maxVit)
    {
        Hp = new Att(maxHp);
        Mp = new Att(maxMp);
        Vit = new Att(maxVit);
    }
    #endregion
}

public class Att
{
    #region 公共字段
    public int Value { get; private set; }
    public int Max { get; private set; }
    public Action<int, int> OnChanged { get; set; }
    #endregion

    #region 构造方法
    public Att(int max, Action<int, int> onChanged = null)
    {
        // 记录回调方法
        OnChanged = onChanged;

        // 设置最大值
        Max = max;

        // 填满
        Full();
    }
    #endregion

    #region 公共方法
    /// <summary>
    /// 填满
    /// </summary>
    public void Full()
    {
        int last = Value;
        Value = Max;

        if (Value != last)
        {
            OnChanged?.Invoke(Value, Max);
        }
    }

    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="value"></param>
    public void Set(int value)
    {
        int last = Value;
        Value = Mathf.Clamp(value, 0, Max);

        if (Value != last)
        {
            OnChanged?.Invoke(Value, Max);
        }
    }

    /// <summary>
    /// 增加值
    /// </summary>
    /// <param name="delta"></param>
    public void Add(int delta)
    {
        int last = Value;
        Value = Mathf.Clamp(last + delta, 0, Max);

        if (Value != last)
        {
            OnChanged?.Invoke(Value, Max);
        }
    }
    #endregion
}
