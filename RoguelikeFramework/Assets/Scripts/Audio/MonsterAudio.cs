using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType {player, smallSlime, goblin, bigSlime, bat, snake, golem, dragon, cardinal, medSlime}

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
        monster.connections.OnDeath.AddListener(1, OnDeath);
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
        
        switch(monster.equipment.equipmentSlots[2].equipped.held[0].sfxType) {
            case SFXItemType.WoodShield: {
                AudioManager.i.WoodenShield(monster.transform);
                break;
            }
            case SFXItemType.MetalShield: {
                AudioManager.i.MetalShield(monster.transform);
                break;
            }
        }

        if(monster.equipment.equipmentSlots[2].equipped.held[1] != null) {
            switch(monster.equipment.equipmentSlots[2].equipped.held[0].sfxType) {
            case SFXItemType.WoodShield: {
                AudioManager.i.WoodenShield(monster.transform);
                break;
            }
            case SFXItemType.MetalShield: {
                AudioManager.i.MetalShield(monster.transform);
                break;
            }
        }
        }

        /*
        foreach(Effect e in item.effects) {
            //effect layers here
        } */
    }

    void TakeDamageSFX(ref int damage, ref DamageType damageType, ref DamageSource source) {
        float maxDamagePct = 0.40f;
        float damageAmt;
        switch (monsterType) {
            case MonsterType.player: 
                Debug.Log(Player.player.stats.resources.health);
                damageAmt = Mathf.Clamp((float)damage/Player.player.stats.resources.health, 0, maxDamagePct) * (1/maxDamagePct);//Player.player.baseStats;
                AudioManager.i.TakeDamage(damageAmt, transform);
                break;
            case MonsterType.goblin:
                AudioManager.i.GoblinDamage(transform);
                break;
            case MonsterType.bigSlime:
                AudioManager.i.BigSlimeDamage(transform);
                break;
            case MonsterType.medSlime:
                AudioManager.i.MedSlimeDamage(transform);
                break;
            case MonsterType.smallSlime:
                AudioManager.i.SmallSlimeDamage(transform);
                break;
            default:
                damageAmt = Mathf.Clamp((float)damage/monster.stats.resources.health, 0, maxDamagePct) * (1/maxDamagePct);//Player.player.baseStats;
                AudioManager.i.TakeDamage(damageAmt, transform);
                break;
                
        }
    }

    void BeginAttackSFX(ref Weapon weapon, ref AttackAction action) {
        switch(weapon.item.sfxType) {
            case SFXItemType.Sword: {
                AudioManager.i.SwordAttack(action.caller.transform);
                break;
            }
            case SFXItemType.Bow: {
                AudioManager.i.BowAttack(action.caller.transform);
                break;
            }
        }

        foreach(Effect e in weapon.item.effects) {
            //effect layers here, i. e.
            if(e is DamageOverTime) {
                //play sfx 
            }
        }

        switch(monsterType) {
            case MonsterType.bigSlime: {
                AudioManager.i.BigSlimeAttack(transform);
                break;
            }
            case MonsterType.medSlime: {
                AudioManager.i.MedSlimeAttack(transform);
                break;
            }
            case MonsterType.smallSlime: {
                AudioManager.i.SmallSlimeAttack(transform);
                break;
            }
            case MonsterType.goblin: {
                AudioManager.i.GoblinAttack(transform);
                break;
            }
        }
    }

    void StopAmbient() {
        //AudioManager.i.StopEvent(ambientSFXInstance);
    }

    void OnDeath() {
        StopAmbient();
        if(monsterType != MonsterType.player)
            AudioManager.i.EnemyDeath(monster.transform);

        switch(monsterType) {
            case MonsterType.smallSlime: {
                AudioManager.i.SmallSlimeSplit(transform);
                break;
            }
            case MonsterType.goblin: {
                AudioManager.i.GoblinDeath(transform);
                break;
            }
        }
    }

    public void SlimeOnSplit() {
        StopAmbient();
        switch(monsterType) {
            case MonsterType.bigSlime: {
                AudioManager.i.BigSlimeSplit(transform);
                break;
            }
            case MonsterType.medSlime: {
                AudioManager.i.MedSlimeSplit(transform);
                break;
            }
            default:
                Debug.Log("Slime split SFX called on non-splittable entity");
                break;
        }
    }

}
