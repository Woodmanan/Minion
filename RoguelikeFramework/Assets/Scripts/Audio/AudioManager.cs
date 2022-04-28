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

        pause = FMODUnity.RuntimeManager.CreateInstance("snapshot:/Pause");
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

    public FMOD.Studio.EventInstance pause;
    int pauseLayers = 0;

    public void Pause() {
        if(pauseLayers == 0) {
            pause.start();
        }
        pauseLayers++;
    }

    public void UnPause() {
        pauseLayers--;
        if(pause.isValid() && pauseLayers == 0) {
            pause.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
    
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

    public async void TakeDamage(float damage, Transform t) {
        FMOD.Studio.EventInstance takeDamage = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Player/Take Damage");
        takeDamage.setParameterByName("AttackDamage", damage);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(takeDamage, t);
        takeDamage.start();
        takeDamage.release();
    }

    public void Staircase() {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Stairs");
    }
    
    //Weapon SFX

    public async void SwordAttack(Transform t) {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Items/Sword Attack", t.position);
    }

    public void BowAttack(Transform t) {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Items/Bow Attack", t.position);
    }

    public void WoodenShield(Transform t) {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Items/Wooden Shield", t.position);
    }

    public void FireLayer(Transform t) {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Items/Fire Layer", t.position);
    }

    //Monster SFX

    public void EnemyDeath(Transform t) {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemies/Death", t.position);
    }

    public bool IsPlaying(FMOD.Studio.EventInstance instance) {
        FMOD.Studio.PLAYBACK_STATE state;   
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }

}
