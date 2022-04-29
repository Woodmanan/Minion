using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "LightPlacer", menuName = "Dungeon Generator/Machines/LightPlacer", order = 2)]
public class LightPlacer : Machine
{
    [Header("Tile numbers")]
    public int lightSourceIndex;

    [Header("Light settings")] //options for color/range?
    public List<int> excludeRoom;

    List<int> roomsToConnect = new List<int>();

    // Activate is called to start the machine
    public override void Activate()
    {
        roomsToConnect.Clear();
        for (int i = 0; i < generator.rooms.Count; i++)
        {
            roomsToConnect.Add(i);
        }

        foreach (int i in excludeRoom)
        {
            roomsToConnect.Remove(i);
        }

        //Shuffle!
        roomsToConnect = roomsToConnect.OrderBy(x => UnityEngine.Random.Range(int.MinValue, int.MaxValue)).ToList();
        UnityEngine.Debug.Log("shuffled");
        //Place Lights
        foreach (int i in roomsToConnect)
        {
            Vector2Int prev = new Vector2Int(-100,-100);
            Vector2Int loc;
            int count = (int)UnityEngine.Random.Range(1f,2.7f);
            for (int j = 0; j < count; j++)
            {
                Room r = generator.rooms[roomsToConnect[i]];
                while (true)
                {
                    loc = r.GetOpenSpace(1, generator.map);
                    if (Math.Abs(prev.x - loc.x) + Math.Abs(prev.y - loc.y) >= 2) 
                        break;
                }

                generator.map[loc.x, loc.y] = lightSourceIndex;
                prev = loc;
            }
        }

    }

}
