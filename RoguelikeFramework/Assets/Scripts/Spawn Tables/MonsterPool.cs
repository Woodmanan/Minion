using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Monster Pool", menuName = "Distribution/Monster Pool", order = 4)]
public class MonsterPool : ScriptableObject
{
    public List<MonsterTable> tables;

    public float chanceForOutOfDepth;
    public int maxDepthIncrease;

    private bool setup = false;
    
    public void SetupTables()
    {
        if (setup) return;
        tables = tables.Select(x => Instantiate(x)).ToList();
        foreach (MonsterTable table in tables)
        {
            table.CalculateDepths();
        }
        setup = true;
    }

    public Monster SpawnMonster(int depth)
    {
        if (!setup) SetupTables();

        //Do out-of-depth check
        if (Random.Range(0.0f, 99.99f) < chanceForOutOfDepth)
        {
            depth += Random.Range(0, maxDepthIncrease);
        }

        //Filter to just tables that can support our query
        List<MonsterTable> options = tables.Where(x => x.containedDepths.Contains(depth)).ToList();

        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        foreach (MonsterTable t in options)
        {
            Debug.Assert(t.containedDepths.Contains(depth), $"{t.name} somehow slipped through the filter");
        }
        #endif

        if (options.Count == 0)
        {
            Debug.LogError($"This pool can't support spawning monsters at depth {depth}!");
            return null;
        }

        MonsterTable chosenTable = options[Random.Range(0, options.Count)];


        return chosenTable.RandomMonsterByDepth(depth);
    }
}
