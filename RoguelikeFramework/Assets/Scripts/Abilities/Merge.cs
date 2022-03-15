using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Merge", menuName = "Abilities/Merge", order = 1)]
public class Merge : Ability
{
    public float minHPPercent = 0.5f;
    public Monster mergeInto;
    private List<Monster> mergeCanidates;

	//Check activation, but for requirements that you are willing to override (IE, needs some amount of gold to cast)
    public override bool OnCheckActivationSoft(Monster caster)
    {
        if (caster.DistanceFrom(Player.player) < 4)
        {
            return false;
        }
        else if (!caster.view.visibleMonsters.Contains(Player.player))
        {
            return true;
        }

        bool healthHighEnough = ((float)caster.resources.health / (float)caster.stats.resources.health) > minHPPercent;
        return healthHighEnough;
    }

    //Check activation, but for requirements that MUST be present for the spell to launch correctly. (Status effects will never override)
    public override bool OnCheckActivationHard(Monster caster)
    {
        bool canCast = false;
        mergeCanidates = new List<Monster>();
        Vector2Int location = caster.location;

        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                // check in 2x2 box minus corners and self
                if (Mathf.Abs(x) + Mathf.Abs(y) == 4)
                    continue;

                Vector2Int checkCoords = location + new Vector2Int(x, y);
                CustomTile checkSpot = Map.current.GetTile(checkCoords);
                if (checkSpot != null && checkSpot.currentlyStanding != null
                    && checkSpot.currentlyStanding.ID == caster.ID
                    && !checkSpot.currentlyStanding.IsDead()
                    && checkSpot.currentlyStanding != caster)
                {
                    canCast = true;
                    mergeCanidates.Add(checkSpot.currentlyStanding);
                }
            }
        }

        return canCast;
    }

    public override void OnCast(Monster caster)
    {
        Monster parent = Instantiate(mergeInto);
        Map.current.GetTile(caster.location).currentlyStanding = parent;
        parent.location = caster.location;
        parent.transform.parent = Map.current.monsterContainer;
        parent.Setup();
        parent.PostSetup(Map.current);
        Map.current.spawnedMonsters.Add(parent);

        // Destroy caster and merge target
        Monster merger = mergeCanidates[Random.Range(0, mergeCanidates.Count)];
        merger.Remove();
        caster.Remove();
    }
}
