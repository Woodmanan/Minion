using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Split", menuName = "Abilities/Split", order = 1)]
public class Split : Ability
{
    public float healthPercentage = 0.5f;
    public Monster splitInto;
    private List<Vector2Int> splitPositionCanidates;

	//Check activation, but for requirements that you are willing to override (IE, needs some amount of gold to cast)
    public override bool OnCheckActivationSoft(Monster caster)
    {
        bool healthLowEnough = ((float)caster.resources.health / (float)caster.stats.resources.health) < healthPercentage;
        return healthLowEnough;
    }

    //Check activation, but for requirements that MUST be present for the spell to launch correctly. (Status effects will never override)
    public override bool OnCheckActivationHard(Monster caster)
    {
        bool canCast = false;
        splitPositionCanidates = new List<Vector2Int>();
        Vector2Int location = caster.location;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int checkCoords = location + new Vector2Int(x, y);
                CustomTile checkSpot = Map.current.GetTile(checkCoords);
                // add spot is canidate if spot is empty
                if (checkSpot != null && checkSpot.currentlyStanding == null
                    && !checkSpot.BlocksMovement())
                {
                    canCast = true;
                    splitPositionCanidates.Add(checkCoords);
                }
            }
        }

        return canCast;
    }

    public override void OnCast(Monster caster)
    {
        splitPositionCanidates.Add(caster.location);
        Monster child1 = Instantiate(splitInto);
        Monster child2 = Instantiate(splitInto);
        int index = Random.Range(0, splitPositionCanidates.Count);
        Map.current.GetTile(splitPositionCanidates[index]).currentlyStanding = child1;
        child1.location = splitPositionCanidates[index];
        child1.transform.parent = Map.current.monsterContainer;
        child1.Setup();
        child1.PostSetup(Map.current);
        splitPositionCanidates.RemoveAt(index);
        index = Random.Range(0, splitPositionCanidates.Count);
        Map.current.GetTile(splitPositionCanidates[index]).currentlyStanding = child2;
        child2.location = splitPositionCanidates[index];
        child2.transform.parent = Map.current.monsterContainer;
        child2.Setup();
        child2.PostSetup(Map.current);
        Map.current.spawnedMonsters.Add(child1);
        Map.current.spawnedMonsters.Add(child2);

        // Destroy caster
        caster.Remove();
    }
}
