using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    #region 公共字段
    #endregion

    #region 私有字段
    /// <summary>
    /// 记录已经检查的位置 
    /// </summary>
    static Dictionary<Vector2Int, CheckData> _dicChecked = new Dictionary<Vector2Int, CheckData>();

    /// <summary>
    /// 检测字典
    /// </summary>
    static Dictionary<Vector2Int, CheckData> _dicCheck = new Dictionary<Vector2Int, CheckData>();
    #endregion

    #region 公共方法
    /// <summary>
    /// 获取路径
    /// </summary>
    /// <param name="mapData"></param>
    /// <param name="startPosIdx"></param>
    /// <param name="targetPosIdx"></param>
    /// <returns></returns>
    public static List<Vector2Int> GetPath(MapTileData[,] mapData, Vector2Int startPosIdx, Vector2Int targetPosIdx)
    {
        _dicChecked.Clear();
        _dicCheck.Clear();
        List<Vector2Int> list = new List<Vector2Int>();

        // 从起点开始检测
        CheckData startData = new CheckData(startPosIdx, targetPosIdx, null);
        _dicChecked.Add(startPosIdx, startData);
        if (CheckNode(mapData, targetPosIdx, startPosIdx, startData))
        {
            // 获取目标位置数据
            CheckData checkData = _dicChecked[targetPosIdx];
            list.Add(checkData.PosIdx);

            // 反向遍历回起点
            do
            {
                // 获取父数据
                checkData = checkData.Parent;
                list.Add(checkData.PosIdx);
            } while (checkData.Parent != null);
        }

        // 反向
        list.Reverse();

        return list;
    }
    #endregion

    #region 其他方法
    /// <summary>
    /// 检测节点
    /// </summary>
    /// <param name="mapData"></param>
    /// <param name="targetPosIdx"></param>
    /// <param name="selfPosIdx"></param>
    /// <param name="parent"></param>
    /// <return></return>
    static bool CheckNode(MapTileData[,] mapData, Vector2Int targetPosIdx, Vector2Int selfPosIdx, CheckData selfData)
    {
        // 获取并添加附近节点
        for (int row = selfPosIdx.x - 1; row <= selfPosIdx.x + 1; row++)
        {
            for (int col = selfPosIdx.y - 1; col <= selfPosIdx.y + 1; col++)
            {
                // 位置无效
                if (row == selfPosIdx.x && col == selfPosIdx.y)
                {
                    continue;
                }
                if (row != selfPosIdx.x && col != selfPosIdx.y)
                {
                    continue;
                }

                // 是否超出范围
                if (row < 0 || col < 0 || row >= MapMgr.Ins.halfHeight || col >= MapMgr.Ins.width)
                {
                    continue;
                }

                // 获得下标
                Vector2Int posIdx = new Vector2Int(row, col);

                // 获取检测数据
                CheckData checkData = new CheckData(posIdx, targetPosIdx, selfData);

                // 是否是目标
                if (posIdx == targetPosIdx)
                {
                    // 记录
                    _dicChecked.Add(posIdx, checkData);

                    return true;
                }

                // 是否不可移动
                if (mapData[row, col].obstacle > 0 || mapData[row, col].character != null)
                {
                    continue;
                }

                // 是否检测过
                if (_dicChecked.ContainsKey(posIdx))
                {
                    continue;
                }

                // 记录
                if (!_dicCheck.ContainsKey(posIdx))
                {
                    _dicCheck.Add(posIdx, checkData);
                }
            }
        }

        // 是否没有数据
        if (_dicCheck.Count == 0)
        {
            // 不连通
            return false;
        }

        // 从列表中取F最小的
        CheckData data = null;
        foreach (KeyValuePair<Vector2Int, CheckData> item in _dicCheck)
        {
            if (data == null || item.Value.F < data.F)
            {
                data = item.Value;
            }
        }

        // 移动数据
        _dicCheck.Remove(data.PosIdx);
        _dicChecked.Add(data.PosIdx, data);

        // 递归
        return CheckNode(mapData, targetPosIdx, data.PosIdx, data);
    }
    #endregion

    #region 内部类
    class CheckData
    {
        /// <summary>
        /// 位置下标
        /// </summary>
        public Vector2Int PosIdx { get; private set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public CheckData Parent { get; private set; }

        /// <summary>
        /// 相对估算 水平移动+10 斜向移动+14
        /// </summary>
        public int G { get; private set; }

        /// <summary>
        /// 直线距离 一格+10
        /// </summary>
        public int H { get; private set; }

        /// <summary>
        /// 总和 每次判断取最小的
        /// </summary>
        public int F
        {
            get
            {
                return G + H;
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="posIdx"></param>
        /// <param name="targetPosIdx"></param>
        /// <param name="parent"></param>
        public CheckData(Vector2Int posIdx, Vector2Int targetPosIdx, CheckData parent)
        {
            PosIdx = posIdx;
            Parent = parent;

            // 有一个方向没移动
            if ((targetPosIdx.x - posIdx.x) * (targetPosIdx.y - posIdx.y) == 0)
            {
                G = 10;
            }

            // 计算H
            H = (Mathf.Abs(targetPosIdx.x - posIdx.x) + Mathf.Abs(targetPosIdx.y - posIdx.y)) * 10;
        }
    }
    #endregion
}
