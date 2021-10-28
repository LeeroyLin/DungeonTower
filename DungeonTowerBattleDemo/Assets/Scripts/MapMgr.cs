using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : SingletonScript<MapMgr>
{
    #region 公共字段
    /// <summary>
    /// 平台组
    /// </summary>
    public Transform[] planes;

    /// <summary>
    /// 预制体组
    /// </summary>
    public Transform[] prefabs;

    /// <summary>
    /// 角色节点
    /// </summary>
    public Transform[] characterNodes;

    /// <summary>
    /// 一个数据格子占真实尺寸
    /// </summary>
    public int perSize = 2;

    /// <summary>
    /// 两个平台间的间隙
    /// </summary>
    public float planeSpacing = 0.25f;

    /// <summary>
    /// 整体地图宽
    /// </summary>
    public int width;

    /// <summary>
    /// 半地图高
    /// </summary>
    public int halfHeight;
    #endregion

    #region 私有字段
    /// <summary>
    /// 成员列表
    /// </summary>
    List<Member> _listMembers = new List<Member>();

    /// <summary>
    /// 敌人列表
    /// </summary>
    List<Enemy> _listEnemies = new List<Enemy>();

    /// <summary>
    /// 己方地图数据
    /// </summary>
    MapTileData[,] _mapMember;

    /// <summary>
    /// 地方地图数据
    /// </summary>
    MapTileData[,] _mapEnemy;
    #endregion

    #region 默认回调
    /// <summary>
    /// 开始后回调
    /// </summary>
    void Start()
    {
        // 设置地图数据
        SetMapData();

        // 创建地图
        CreateMap();

        // 设置角色位置
        SetCharacterPos();
    }

    /// <summary>
    /// 每帧回调
    /// </summary>
    void Update()
    {
        
    }
    #endregion

    #region 公共方法
    /// <summary>
    /// 添加队员
    /// </summary>
    /// <param name="member"></param>
    /// <param name="rowIdx"></param>
    /// <param name="colIdx"></param>
    public void AddMemberCharacter(Member member, int rowIdx, int colIdx)
    {
        member.SetPosIdx(rowIdx, colIdx);
        _listMembers.Add(member);
        int idx = _listMembers.Count - 1;

        _mapMember[rowIdx, colIdx].SetCharacter(idx, true);
    }

    /// <summary>
    /// 添加敌人
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="rowIdx"></param>
    /// <param name="colIdx"></param>
    public void AddEnemyCharacter(Enemy enemy, int rowIdx, int colIdx)
    {
        enemy.SetPosIdx(rowIdx, colIdx);
        _listEnemies.Add(enemy);
        int idx = _listEnemies.Count - 1;

        _mapEnemy[rowIdx, colIdx].SetCharacter(idx, true);
    }
    #endregion

    #region 其他方法
    /// <summary>
    /// 设置地图数据
    /// </summary>
    void SetMapData()
    {
        // 初始化地图列表
        _mapMember = new MapTileData[halfHeight, width];
        _mapEnemy = new MapTileData[halfHeight, width];
        for (int i = 0; i < halfHeight; i++)
        {
            for (int t = 0; t < width; t++)
            {
                _mapMember[i, t] = new MapTileData(i, t);
                _mapEnemy[i, t] = new MapTileData(i, t);
            }
        }

        // 添加角色
        AddMemberCharacter(new Member("阿巴阿巴"), 1, 4);
        AddEnemyCharacter(new Enemy("阿西吧"), 1, 2);
        AddEnemyCharacter(new Enemy("西八"), 1, 6);
    }

    /// <summary>
    /// 创建地图
    /// </summary>
    void CreateMap()
    {
        // 设置己方和敌人平面
        for (int i = 0; i < 2; i++)
        {
            SetPlane(i);
        }
    }

    /// <summary>
    /// 设置平面
    /// </summary>
    /// <param name="idx"></param>
    void SetPlane(int idx)
    {
        // 获取平面对象
        Transform plane = planes[idx];

        // 缩放平面
        plane.localScale = new Vector3(width / 10f * perSize, 1, halfHeight / 10f * perSize);

        // 设置材质
        plane.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(width, halfHeight));

        // 设置位置
        plane.position = new Vector3(0, 0, (idx == 0 ? -1 : 1) * (halfHeight * perSize + planeSpacing) * 0.5f);
    }

    /// <summary>
    /// 设置角色位置
    /// </summary>
    void SetCharacterPos()
    {
        // 遍历敌人列表
        Transform enemy;
        foreach (var item in _listEnemies)
        {
            // 生成敌人预制体
            enemy = Instantiate(prefabs[0]);

            // 设置父节点
            enemy.SetParent(characterNodes[0], false);

            // 设置位置
            enemy.position = GetCharacterPos(item);

            // 设置转向
            enemy.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        }

        // 遍历成员列表
        Transform member;
        foreach (var item in _listMembers)
        {
            // 生成敌人预制体
            member = Instantiate(prefabs[1]);

            // 设置父节点
            member.SetParent(characterNodes[1], false);

            // 设置位置
            member.position = GetCharacterPos(item);

            // 设置转向
            member.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
    }

    /// <summary>
    /// 获取角色位置
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    Vector3 GetCharacterPos(BaseCharacter character)
    {
        Vector3 pos = Vector3.zero;

        // 计算平台中心z位置
        float planeZ = character.isMember ? planes[0].position.z : planes[1].position.z;

        pos.x = -width * 0.5f * perSize + (0.5f + character.colIdx) * perSize;
        pos.z = planeZ + -halfHeight * 0.5f * perSize + (0.5f + character.rowIdx) * perSize;

        return pos;
    }
    #endregion
}
