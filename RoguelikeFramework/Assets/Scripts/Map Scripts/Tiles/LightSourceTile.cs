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
    public void Cast(CustomTile tile, int lvl)
    {
        if (!tile.blocksVision) 
        {
            //walls should probably be lit when shader is written, but looks bad rn.
            tile.lightLevel = lvl;

            int x = tile.location.x;
            int y = tile.location.y;
        
            CustomTile[] neighbors = new CustomTile[] 
            {map.GetTile(new Vector2Int(x,y-1)),
            map.GetTile(new Vector2Int(x,y+1)),
            map.GetTile(new Vector2Int(x+1,y)),
            map.GetTile(new Vector2Int(x-1,y))};

            foreach(CustomTile nb in neighbors)
            {
                if (lvl > nb.lightLevel)
                    Cast(nb, lvl - 1); 
            }
        }
    }

    //Called at the end of map construction, once this tile is guarunteed to be in the map!
    public override void SetInMap(Map m)
    {
        Cast(this, lightLevel);
    }
}
