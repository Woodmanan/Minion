using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorativeTile : CustomTile
{
    [SerializeField] List<Sprite> decorations;
    [SerializeField] private int decorateOneIn;
    private bool setupDecoration = false;

    public override void RebuildGraphics()
    {
        if (!setupDecoration && decorations.Count > 0)
        {
            if (Random.Range(0, decorateOneIn) == 0)
            {
                render.sprite = decorations[Random.Range(0, decorations.Count)];
            }
            
        }

        setupDecoration = true;

        base.RebuildGraphics();
    }
}
