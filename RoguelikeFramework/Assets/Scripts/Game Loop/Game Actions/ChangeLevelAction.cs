﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevelAction : GameAction
{
    bool up;

    //Constuctor for the action; must include caller!
    public ChangeLevelAction(bool up)
    {
        this.up = up;
    }

    //The main function! This EXACT coroutine will be executed, even across frames.
    //See GameAction.cs for more information on how this function should work!
    public override IEnumerator TakeAction()
    {
        Debug.Log("Moving levels!");
        Stair stair = Map.current.GetTile(caller.location) as Stair;
        if (stair)
        {
            if (stair.upStair ^ up)
            {
                yield break;
                /*bool keepGoing = false;
                UIController.singleton.OpenConfirmation($"<color=\"black\">Are you sure you want to go {(up ? "up" : "down")}?", (b) => keepGoing = b);
                yield return new WaitUntil(() => !UIController.WindowsOpen);
                if (!keepGoing) yield break;*/
            }

            if (stair.connectsToFloor >= LevelLoader.singleton.generators.Count)
            {
                //We win!
                SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
                yield break;
            }

            AudioManager.i.Staircase();
            GameController.singleton.MoveToLevel(stair.connectsToFloor);
            caller.energy -= 100;

        }
        else
        {
            Debug.Log($"Console: You cannot go {(up ? "up" : "down")} here!");
        }
    }

    //Called after construction, but before execution!
    //This is THE FIRST spot where caller is not null! Heres a great spot to actually set things up.
    public override void OnSetup()
    {

    }
}
