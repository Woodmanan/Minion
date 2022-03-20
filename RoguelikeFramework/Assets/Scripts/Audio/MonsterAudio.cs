using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType {player, slime}

public class MonsterAudio : MonoBehaviour
{
    
    public MonsterType monsterType;
    Monster monster;

    FMOD.Studio.EventInstance ambientSFXInstance;

    // Start is called before the first frame update
    public virtual void Start()
    {
        monster = GetComponent<Monster>();
        StartAmbientSFX();
        monster.connections.OnMove.AddListener(1, MoveSFX);
        monster.connections.OnTakeDamage.AddListener(1, TakeDamageSFX);
        monster.connections.OnStartAttack.AddListener(1, StartAttackSFX);
    }

    void OnDestroy() {
        StopAmbient();
    }

    void StartAmbientSFX() {
        switch (monsterType) {
            case MonsterType.player:
                //put SFX here
                break;
        }
    }

    void MoveSFX() {
        switch (monsterType) {
            case MonsterType.player:
                AudioManager.i.Footstep();
                break;
        }
    }

    void TakeDamageSFX(ref int damage, ref DamageType damageType, ref DamageSource source) {
        switch (monsterType) {
            case MonsterType.player:
                //put SFX here
                break;
        }
    }

    void StartAttackSFX(ref AttackAction action, ref bool canContinue) {
        switch (monsterType) {
            case MonsterType.player:
                //put SFX here
                break;
        }
    }

    void StopAmbient() {
        //AudioManager.i.StopEvent(ambientSFXInstance);
    }

    void OnDeath() {
        StopAmbient();
        //AudioManager.i.MonsterDeath(monster.transform);
    }

}
