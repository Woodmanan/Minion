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
        UISelect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMusic(int newFloorNum) {
        level = newFloorNum;
        //switch music using level variable
        string eventString;
        switch(level)
        {
            case 1:
                eventString = "event:/Music/Level 1 Music";
                break;
            case 2:
                eventString = "event:/Music/Jungle Music";
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
        if(level == 4) {
            Music.setParameterByName("Enemies", 2);
        }
        Music.release();
    }

    //UI SFX

    public void UIMouseover() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Mouseover");
    }

    public void UISelect() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Select");
    }
}
