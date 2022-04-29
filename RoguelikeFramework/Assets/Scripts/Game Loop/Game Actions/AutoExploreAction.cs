﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoExploreAction : GameAction
{
    //Constuctor for the action; must include caller!
    public AutoExploreAction()
    {
        //Construct me! Assigns caller by default in the base class
    }

    //The main function! This EXACT coroutine will be executed, even across frames.
    //See GameAction.cs for more information on how this function should work!
    public override IEnumerator TakeAction()
    {
        while (true)
        {
            if (caller.view.visibleMonsters.FindAll(x => x.IsEnemy(caller)).Count > 0)
            {
                LogManager.S.Log("You cannot auto-explore while enemies are in sight.");
                yield break;
            }

            //TODO: Rest action first!
            GameAction restAct = new RestAction();
            restAct.Setup(caller);
            while (restAct.action.MoveNext())
            {
                yield return restAct.action.Current;
            }

            //Build up the points we need!
            List<Vector2Int> goals = new List<Vector2Int>();
            for (int i = 1; i < Map.current.width - 1; i++)
            {
                for (int j = 1; j < Map.current.height - 1; j++)
                {
                    Vector2Int pos = new Vector2Int(i, j);
                    if (Map.current.NeedsExploring(pos))
                    {
                        goals.Add(pos);
                    }
                }
            }

            if (goals.Count == 0)
            {
                LogManager.S.Log("There's nothing else to explore!");
                
                yield break;
            }

            Path path = Pathfinding.CreateDjikstraPath(caller.location, goals.ToArray());

            if (path.Count() == 0)
            {
                Debug.Log("Can't reach anymore spaces!");
                yield break;
            }

            while (path.Count() > 0)
            {
                Vector2Int next = path.Pop();
                MoveAction act = new MoveAction(next, true, false);

                caller.UpdateLOS();

                if (caller.view.visibleMonsters.FindAll(x => x.IsEnemy(caller)).Count > 0)
                {
                    LogManager.S.Log($"You stop.");
                    yield break;
                }

                act.Setup(caller);
                while (act.action.MoveNext())
                {
                    yield return act.action.Current;
                }

                yield return new WaitForSeconds(.05f);

                yield return GameAction.StateCheck;
            }
        }
    }

    //Called after construction, but before execution!
    //This is THE FIRST spot where caller is not null! Heres a great spot to actually set things up.
    public override void OnSetup()
    {

    }
}
