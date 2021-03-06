using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager i;
    private static FMOD.Studio.EventInstance Music;
    public int level = 1;
    

    void Awake()
     {
         if(i != null)
            GameObject.Destroy(i);
         else
            i = this;

         DontDestroyOnLoad(this);
     }

    // Start is called before the first frame update
    void Start()
    {
        StartMusic(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMusic(bool isInDanger) {
        
        float newVal = 0f;
        if(isInDanger) {
            newVal = 1f;
        }
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("InDanger", newVal);
    }

    public void StartMusic(int levelNum) {
        level = levelNum;
        //switch music using level variable
        string eventString;
        switch(level)
        { 
            case 1:
                eventString = "event:/Music/Level 1 Music";
                break;
            case 2:
                eventString = "event:/Music/Forest Music";
                break;
            case 3:
                eventString = "event:/Music/Level 3 Music";
                break;
            case 4:
                eventString = "event:/Music/Level 3 Music";
                break;
            default:
                eventString = "event:/Music/Level 1 Music";
                break;
        }
        Music = FMODUnity.RuntimeManager.CreateInstance(eventString);
        Music.start();
    }

    public void GameOver() {
        StopMusic();
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Game Over");
        Music.release();
    }

    public void StopMusic() {
        Music.release();
        Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    //UI SFX

    public void UIMouseover() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Mouseover");
    }

    public void UISelect() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Start");
    }

    //Player SFX

    public void Footstep() {
        Debug.Log("i take a stepi");
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Footstep");
    }
}
