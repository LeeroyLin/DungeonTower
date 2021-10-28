using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileData
{
    public int rowIdx;
    public int colIdx;
    public bool isMember;
    public int characterIdx;
    public bool isCharacterMoving;

    public MapTileData(int rowIdx, int colIdx)
    {
        this.rowIdx = rowIdx;
        this.colIdx = colIdx;
    }

    public void SetCharacter(int characterIdx, bool isMember)
    {
        this.characterIdx = characterIdx;
        this.isMember = isMember;
    }
}
