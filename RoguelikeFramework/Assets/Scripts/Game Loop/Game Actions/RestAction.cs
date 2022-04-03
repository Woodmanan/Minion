using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAction : GameAction
{
    //Constuctor for the action
    public RestAction()
    {
        //Construct me! Don't need caller here, that will get assigned during Setup.
    }

    //The main function! This EXACT coroutine will be executed, even across frames.
    //See GameAction.cs for more information on how this function should work!
    public override IEnumerator TakeAction()
    {
        if (caller.resources.health == caller.stats.resources.health)
        {
            yield break;
        }

        while (true)
        {
            
            List<Monster> enemies = caller.view.visibleMonsters.FindAll(x => x.IsEnemy(caller));
            if (enemies.Count > 0)
            {
                Debug.Log("Console: You stop resting.");
                yield break;
            }

            yield return null;

            GameAction act = new WaitAction();
            act.Setup(caller);
            while (act.action.MoveNext())
            {
                yield return act.action.Current;
            }

            if (caller.resources.health == caller.stats.resources.health)
            {
                Debug.Log("Console: You finish resting.");
                yield break;
            }
        }
    }

    //Called after construction, but before execution!
    //This is THE FIRST spot where caller is not null! Heres a great spot to actually set things up.
    public override void OnSetup()
    {

    }
}
