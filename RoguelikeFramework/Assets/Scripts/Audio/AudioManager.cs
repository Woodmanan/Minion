using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager i;
    private static FMOD.Studio.EventInstance Music;
    private int level = 0;
    public int Level {
        get { return level; }
        set {
            StopMusic();
            level = value;
            StartMusic(level);
        }
    }

    void Awake()
     {
         if(i != null) {
            GameObject.Destroy(this);
         }
         else {
            i = this;

            DontDestroyOnLoad(this);
         }

     }

    // Start is called before the first frame update
    void Start()
    {
        StartMusic(level);
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
        StopMusic();
        //switch music using level variable
        UpdateMusic(false);
        string eventString;
        switch(levelNum)
        { 
            case 0:
                eventString = "event:/Music/Main Menu";
                break;
            case 1:
                eventString = "event:/Music/Dungeon Music";
                break;
            case 2:
                eventString = "event:/Music/Forest Music";
                break;
            case 3:
                eventString = "event:/Music/Level 3 Music";
                break;
            case 4:
                eventString = "event:/Music/Dungeon Boss Music";
                break;
            default:
                eventString = "event:/Music/Level 1 Music";
                break;
        }
        Music = FMODUnity.RuntimeManager.CreateInstance(eventString);
        Music.start();
    }

    public void StartBossMusic() {
        StopMusic();
        StartMusic(level + 3);
    }

    public void GameOver() {
        StopMusic();
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Game Over");
        Music.start();
        Music.release();
    }

    public void StopMusic() {
        if(Music.isValid()) {
            Music.release();
            Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    //UI SFX
    
    public void UISelect() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Select");
    }

    public void UIMouseover() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Mouseover");
    }

    public void UIStart() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Start");
    }

    //Player SFX

    public void Footstep() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Footstep");
    }

    public void HealthUp(float totalHealth, float healthAdded) {
        Debug.Log(totalHealth + " and adding " + healthAdded);
        FMOD.Studio.EventInstance healthUp = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Player/Health Up");
        healthUp.setParameterByName("Health", totalHealth);
        healthUp.setParameterByName("healthAdded", healthAdded);
        healthUp.start();
        healthUp.release();
    }

    public void TakeDamage(float damage) {
        Debug.Log("aasas " + damage);
        FMOD.Studio.EventInstance takeDamage = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Player/Take Damage");
        takeDamage.setParameterByName("AttackDamage", damage);
        takeDamage.start();
        takeDamage.release();
    }

    public void Staircase() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Stairs");
    }
    
    //Weapon SFX

    public void SwordAttack() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Items/Sword Attack");
    }

    public void BowAttack() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Items/Bow Attack");
    }

    public void WoodenShield() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Items/Wooden Shield");
    }

    public void FireLayer() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Items/Fire Layer");
    }

}
