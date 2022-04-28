using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : RogueUIPanel
{
    //Don't uncomment these! These are already declared in the base class,
    //and are listed here so you know they exist.

    //bool inFocus; - Tells you if this is the window that is currently focused. Not too much otherwise.

    

    /*
     * One of the more important functions here. When in focus, this will be called
     * every frame with the stored input from InputTracking
     */
    public override void HandleInput(PlayerAction action, string inputString)
    {
       
    }

    /* Called every time this panel is activated by the controller */
    public override void OnActivation()
    {
        AudioManager.i.Pause();
    }
    
    /* Called every time this panel is deactived by the controller */
    public override void OnDeactivation()
    {
        AudioManager.i.UnPause();
    }

    /* Called every time this panel is focused on. Use this to refresh values that might have changed */
    public override void OnFocus()
    {

    }

    /*
     * Called when this panel is no longer focused on (added something to the UI stack). I don't know 
     * what on earth this would ever get used for, but I'm leaving it just in case (Nethack design!)
     */
    public override void OnDefocus()
    {
        
    }

    public void Resume()
    {
        ExitTopLevel();
    }

    public void ExitGame()
    {
        AudioManager.i.Level = 0;
        SceneManager.LoadScene(0);
    }    
}
