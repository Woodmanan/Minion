using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType {player, slime, goblin, bigSlime}

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
        monster.connections.OnBeginPrimaryAttack.AddListener(1, BeginAttackSFX);
        monster.connections.OnBeginSecondaryAttack.AddListener(1, BeginAttackSFX);
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

    void AttackedSFX() {
        //how to do shield sfx :sob:
        /*
        switch(monster.equipment.) {
            case SFXItemType.WoodShield: {
                AudioManager.i.WoodenShield();
                break;
            }
        }

        foreach(Effect e in item.effects) {
            //effect layers here
        } */
    }

    void TakeDamageSFX(ref int damage, ref DamageType damageType, ref DamageSource source) {
        switch (monsterType) {
            case MonsterType.player: 
                float maxDamagePct = 0.40f;
                Debug.Log(Player.player.stats.resources.health);
                float damageAmt = Mathf.Clamp((float)damage/Player.player.stats.resources.health, 0, maxDamagePct) * (1/maxDamagePct);//Player.player.baseStats;
                AudioManager.i.TakeDamage(damageAmt);
                break;
        }
    }

    void BeginAttackSFX(ref Weapon weapon, ref AttackAction action) {
        switch(weapon.item.sfxType) {
            case SFXItemType.Sword: {
                AudioManager.i.SwordAttack();
                break;
            }
            case SFXItemType.Bow: {
                AudioManager.i.BowAttack();
                break;
            }
        }

        foreach(Effect e in weapon.item.effects) {
            //effect layers here, i. e.
            if(e is DamageOverTime) {
                //play sfx 
            }
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
