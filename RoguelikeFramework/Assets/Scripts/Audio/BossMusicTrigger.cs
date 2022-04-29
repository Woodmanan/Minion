using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicTrigger : MonoBehaviour
{
    MonsterAI monsterAI;
    static bool bossPresent = false;
    bool bossMusicPlaying = false;
    
    // Start is called before the first frame update
    void Start()
    {
        monsterAI = GetComponent<MonsterAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bossPresent != monsterAI.isInBattle) {
            StopAllCoroutines();
            if(!monsterAI.isInBattle) {
                StartCoroutine(TurnOffMusic());
            } else if(!bossMusicPlaying) {
                TurnOnMusic();
            }

            bossPresent = monsterAI.isInBattle;
        }
    }

    void TurnOnMusic() {
        AudioManager.i.StartBossMusic();
        bossMusicPlaying = true;
    }

    IEnumerator TurnOffMusic() {
        yield return new WaitForSeconds(7);
        AudioManager.i.StartMusic(AudioManager.i.Level);
        bossMusicPlaying = false;
    }
}
