﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFloatUI : MonoBehaviour
{
    #region 公共字段
    public Transform transHp;
    public Transform transMp;
    public Transform transVit;
    public Text textName;
    public Transform target;
    #endregion

    #region 私有字段
    RectTransform _rectTrans;
    #endregion

    #region 默认回调
    /// <summary>
    /// 唤醒后回调
    /// </summary>
    private void Awake()
    {
        _rectTrans = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 开始后回调
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// 每帧后回调
    /// </summary>
    void LateUpdate()
    {
        // 跟随
        Follow();
    }
    #endregion

    #region 公共方法
    public void SetHp(int hp, int max)
    {
        transHp.localScale = new Vector3(hp / (float)max, 1, 1);
    }
    public void SetMp(int mp, int max)
    {
        transMp.localScale = new Vector3(mp / (float)max, 1, 1);
    }
    public void SetVit(int vit, int max)
    {
        transVit.localScale = new Vector3(vit / (float)max, 1, 1);
    }
    public void SetName(string name)
    {
        textName.text = name;
    }
    #endregion

    #region 其他方法
    /// <summary>
    /// 跟随
    /// </summary>
    void Follow()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPos = target.position;
        targetPos.y = 1;
        Vector3 viewport = Camera.main.WorldToViewportPoint(targetPos);
        viewport.x -= 0.5f;
        viewport.y -= 0.5f;
        _rectTrans.anchoredPosition = new Vector2(viewport.x * Screen.width, viewport.y * Screen.height);
    }
    #endregion
}
