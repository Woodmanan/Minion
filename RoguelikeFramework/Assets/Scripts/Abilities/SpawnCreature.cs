using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnCreature", menuName = "Abilities/SpawnCreature", order = 1)]
public class SpawnCreature : Ability
{
    private List<Vector2Int> splitPositionCanidates = new List<Vector2Int>();
    public GameObject toSpawn;
    public int numSpawned = 1;

    //Check activation, but for requirements that you are willing to override (IE, needs some amount of gold to cast)
    public override bool OnCheckActivationSoft(Monster caster)
    {
        return true;
    }

    public override bool OnCheckActivationHard(Monster caster)
    {
        bool canCast = false;
        splitPositionCanidates = new List<Vector2Int>();
        Vector2Int location = caster.location;

        for (int x = -targeting.radius; x <= targeting.radius; x++)
        {
            for (int y = -targeting.radius; y <= targeting.radius; y++)
            {
                Vector2Int checkCoords = location + new Vector2Int(x, y);
                Debug.Log("HEre");
                if (!(checkCoords.x >= 0 && checkCoords.x < Map.current.width && checkCoords.y >= 0 && checkCoords.y < Map.current.height)) continue;
                CustomTile checkSpot = Map.current.GetTile(checkCoords);
                Debug.Log("HEre2");
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
        for (int i = 0; i < numSpawned; i++)
        {
            Monster child = Instantiate(toSpawn).GetComponent<Monster>();
            child.faction = caster.faction;
            int index = Random.Range(0, splitPositionCanidates.Count);
            Map.current.GetTile(splitPositionCanidates[index]).currentlyStanding = child;
            child.location = splitPositionCanidates[index];
            child.transform.parent = Map.current.monsterContainer;
            child.Setup();
            child.PostSetup(Map.current);
            splitPositionCanidates.RemoveAt(index);
            Map.current.spawnedMonsters.Add(child);
        }
    }
}
