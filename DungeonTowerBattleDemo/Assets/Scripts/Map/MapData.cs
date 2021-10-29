using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileData
{
    public int rowIdx;
    public int colIdx;
    public bool isMember;
    public BaseCharacter character;
    public int obstacle;

    public MapTileData(int rowIdx, int colIdx)
    {
        this.rowIdx = rowIdx;
        this.colIdx = colIdx;
    }
}
