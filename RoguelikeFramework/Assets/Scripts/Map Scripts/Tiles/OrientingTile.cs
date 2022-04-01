/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientingTile : CustomTile
{
    [SerializeField] Sprite BR_Corner;
    [SerializeField] Sprite R_Wall;
    [SerializeField] Sprite TR_Corner;
    [SerializeField] Sprite B_Wall;
    [SerializeField] Sprite TL_Corner;
    [SerializeField] Sprite L_Wall;
    [SerializeField] Sprite BL_Corner;
    [SerializeField] Sprite T_Wall;

    private bool setupDecoration = false;

    public override void RebuildShape()
    {
        int mask = 0;
        mask = mask + (NeighborBlocks(Vector2Int.up) ? 1 : 0);
        mask = mask + (NeighborBlocks(Vector2Int.right) ? 2 : 0);
        mask = mask + (NeighborBlocks(Vector2Int.down) ? 4 : 0);
        mask = mask + (NeighborBlocks(Vector2Int.left) ? 8 : 0);
        render.sprite = ChooseSprite(mask);
    }

    private Sprite ChooseSprite(int mask)
    {
#if UNITY_EDITOR
        Debug.Assert(mask < 16, "mask cannot be >= 16! Something has gone wrong with orienting tile.", this);
#endif

        switch (mask)
        {
            case 0:
                return L_Wall;
            case 1:
                return L_Wall;
            case 2:
                return B_Wall;
            case 3:
                return TR_Corner;
            case 4:
                return L_Wall;
            case 5:
                return L_Wall;
            case 6:
                return BR_Corner;
            case 7:
                return R_Wall;
            case 8:
                return B_Wall;
            case 9:
                return TL_Corner;
            case 10:
                return B_Wall;
            case 11:
                return T_Wall;
            case 12:
                return BL_Corner;
            case 13:
                return L_Wall;
            case 14:
                return B_Wall;
            case 15:
                return DetermineOpenCorner();
        }
        Debug.LogError("Switch didn't go through");
        return BR_Corner;
    }

    private Sprite DetermineOpenCorner()
    {
        if (!NeighborBlocks(Vector2Int.right + Vector2Int.up))
        {
            return TR_Corner;
        }
        else if (!NeighborBlocks(Vector2Int.left + Vector2Int.up))
        {
            return TL_Corner;
        }
        else if (!NeighborBlocks(Vector2Int.right + Vector2Int.down))
        {
            return BR_Corner;
        }
        else if (!NeighborBlocks(Vector2Int.left + Vector2Int.down))
        {
            return BL_Corner;
        }
        return B_Wall;
    }

    private bool NeighborBlocks(Vector2Int offset)
    {
        CustomTile t = Map.singleton.GetTile(new Vector2Int(x, y) + offset);
        var isOrienting = t as OrientingTile;
        return isOrienting != null;
    }
}
*/