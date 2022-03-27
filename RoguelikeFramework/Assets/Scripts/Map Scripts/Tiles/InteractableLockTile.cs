using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLockTile : InteractableTile
{
    /*public override float Inspect(Monster toInspect)
    {
        return base.Inspect(toInspect);
    }*/

    [SerializeField] Sprite replacementSprite;
    public override GameAction GetAction()
    {
        ActionPlan plan = new ActionPlan();
        plan.AddAction(new PathfindAction(location));
        return plan;
    }

    public override IEnumerator Interact(Monster caller)
    {
        IEnumerable<Item> inventory = caller.inventory.AllHeld();
        foreach (Item i in inventory){
            if (i.type == ItemType.KEY){
                this.movementCost = 1;
                this.render.sprite = replacementSprite;
                Color green = new Color(4.0f / 255, 166.0f / 255, 94.0f / 255);
                this.render.color = green;
                this.color = green;
                this.dirty = true;
                RebuildGraphics();
                caller.inventory.RemoveAt(caller.inventory.GetIndexOf(i));
            }
        }
        yield break;
    }
}
