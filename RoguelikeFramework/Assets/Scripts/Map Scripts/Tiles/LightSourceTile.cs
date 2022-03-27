using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LightSourceTile : CustomTile
{
    [HideInInspector] public LOSData view;

    public void Cast(LOSData ld)
    {
        Vector2Int origin = ld.origin;
        int radius = ld.radius;
        bool[,] definedArea = ld.definedArea;
        Vector2Int start = origin - Vector2Int.one * radius;
        for (int i = 0; i < (radius * 2 + 1); i++)
        {
            for (int j = 0; j < (radius * 2 + 1); j++)
            {
                if (definedArea[i, j])
                {
                    Vector2Int pos = new Vector2Int(i + start.x, j + start.y);
                    int lvl = radius - Math.Abs(origin.x - pos.x) - Math.Abs(origin.y - pos.y);
                    if (lvl > 0) map.GetTile(pos).lightLevel = lvl; //add check for !blocksVision here if you don't want walls lit
                }
            }
        }
    }

    //Called at the end of map construction, once this tile is guarunteed to be in the map!
    public override void SetInMap(Map m)
    {
        view = LOS.LosAt(m, location, lightLevel-1);
        Cast(view);
        if (!isVisible) isHidden = true;
    }
}
