﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EffectGroup("General Effects")]
[CreateAssetMenu(fileName = "New DamageResistance", menuName = "Status Effects/DamageResistance", order = 1)]
public class DamageResistance : Effect
{
    public DamageType resistedType;
    public DamageSource resistedSource;
    public int numTurns;
    public int shavedDamage = 0;
    public float percentDamage = 20f;

    float storedDamage = 0;
    //Constuctor for the object; use this in code if you're not using the asset version!
    public DamageResistance(int numTurns)
    {
        //Construct me!
        this.numTurns = numTurns;
    }

    //Called the moment an effect connects to a monster
    //Use this to apply effects or stats immediately, before the next frame
    /*public override void OnFirstConnection() {}*/

    //Called at the start of the global turn sequence
    public override void OnTurnStartGlobal() 
    {
        numTurns--;
        if (numTurns == 0)
        {
            Disconnect();
        }
    }

    //Called at the end of the global turn sequence
    /*public override void OnTurnEndGlobal() {}*/

    //Called at the start of a monster's turn
    /*public override void OnTurnStartLocal() {}*/

    //Called at the end of a monster's turn
    /*public override void OnTurnEndLocal() {}*/

    //Called whenever a monster takes a step
    /*public override void OnMove() {}*/

    //Called whenever a monster returns to full health
    /*public override void OnFullyHealed() {}*/

    //Called when a monster dies
    /*public override void OnDeath() {}*/

    //Called when a monster is looking to recheck the stats, good for adding in variable stats mid-effect
    //it gains from effects and items
    /*public override void RegenerateStats(ref StatBlock stats) {}*/

    //Called whenever a monster gains energy
    /*public override void OnEnergyGained(ref int energy) {}*/

    //Called when a monster recieves an attack type request
    /*public override void OnAttacked(ref int pierce, ref int accuracy) {}*/

    //Called when a monster takes damage from any source, good for making effects fire upon certain types of damage
    public override void OnTakeDamage(ref int damage, ref DamageType damageType, ref DamageSource source)
    {
        //Some weird float math that tries to keep the damage accurate
        if ((damageType & resistedType) > 0 && (source & resistedSource) > 0)
        {
            storedDamage += (damage * percentDamage / 100);
            while (storedDamage > 1 && damage > 1)
            {
                damage--;
                storedDamage -= 1.0f;
            }
            damage = damage - shavedDamage;
        }
    }

    //Called when a monster recieves a healing event request
    /*public override void OnHealing(ref int healAmount) {}*/

    //Callen when a monster recieves an event with new status effects
    /*public override void OnApplyStatusEffects(ref Effect[] effects) {}*/
}
