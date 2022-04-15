using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StatChange", menuName = "Status Effects/StatChange", order = 1)]
public class StatChange : Effect
{
    [Header("Stat Changes")]
    [SerializeField] private float hpChange;
    [SerializeField] private float strChange;
    [SerializeField] private float dexChange;
    [SerializeField] private float acChange;
    [SerializeField] private float evChange;
    [SerializeField] private float speedChange;
    [SerializeField] private bool isPermanent;
    [SerializeField] private bool isMultiplier;

    //Constuctor for the object; use this in code if you're not using the asset version!
    public StatChange(float hpChange, float strChange, float dexChange, float acChange, float evChange, float speedChange, bool isPermanent, bool isMultiplier, int minNumTurns, int maxNumTurns, bool useMinNumTurnsOnly, bool affectInfinitely, int chanceToBreakEffect, Color effectColor)
    {
        this.hpChange = hpChange;
        this.strChange = strChange;
        this.dexChange = dexChange;
        this.acChange = acChange;
        this.evChange = evChange;
        this.speedChange = speedChange;
        this.isPermanent = isPermanent;
        this.isMultiplier = isMultiplier;
    }

    //Called the moment an effect connects to a monster
    //Use this to apply effects or stats immediately, before the next frame
    public override void OnConnection() {
        base.OnConnection();
        LogStatsChange(connectedTo.monster.baseStats);
        if (isPermanent)
        {
            if (isMultiplier)
            {
                int mh = (int) ((int) (connectedTo.monster.baseStats.resources.health * hpChange) - hpChange);
                connectedTo.monster.baseStats.resources.health = (int) (connectedTo.monster.baseStats.resources.health * hpChange);
                connectedTo.monster.resources.health += mh;

                connectedTo.monster.baseStats.ac = (int) (connectedTo.monster.baseStats.ac * acChange);
                connectedTo.monster.baseStats.ev = (int) (connectedTo.monster.baseStats.ev * evChange);
            }
            else
            {
                int mh = (int) ((int) (connectedTo.monster.baseStats.resources.health + hpChange) - hpChange);
                connectedTo.monster.baseStats.resources.health = (int)(connectedTo.monster.baseStats.resources.health + hpChange);
                connectedTo.monster.resources.health += mh;

                connectedTo.monster.baseStats.ac = (int)(connectedTo.monster.baseStats.ac + acChange);
                connectedTo.monster.baseStats.ev = (int)(connectedTo.monster.baseStats.ev + evChange);
            }
            Disconnect();
        }
    }
    
    /*public override void OnDisconnection() {}*/

    //Called at the start of the global turn sequence
    /*public override void OnTurnStartGlobal() {}*/

    //Called at the end of the global turn sequence
    /*public override void OnTurnEndGlobal() {}*/

    //Called at the start of a monster's turn
    /*public override void OnTurnStartLocal() {}*/

    //Called at the end of a monster's turn
    /*public override void OnTurnEndLocal() {}*/

    //Called whenever a monster returns to full health
    /*public override void OnFullyHealed() {}*/

    //Called when a monster dies
    /*public override void OnDeath() {}*/

    //Called when a monster is looking to recheck the stats, good for adding in variable stats mid-effect
    //it gains from effects and items
    public override void RegenerateStats(ref StatBlock stats) {
        if (!isPermanent)
        {
            if (isMultiplier)
            {
                int mh = (int)((int)(connectedTo.monster.baseStats.resources.health * hpChange) - hpChange);
                connectedTo.monster.baseStats.resources.health = (int)(connectedTo.monster.baseStats.resources.health * hpChange);
                connectedTo.monster.resources.health += mh;

                connectedTo.monster.baseStats.ac = (int)(connectedTo.monster.baseStats.ac * acChange);
                connectedTo.monster.baseStats.ev = (int)(connectedTo.monster.baseStats.ev * evChange);
            }
            else
            {
                int mh = (int)((int)(connectedTo.monster.baseStats.resources.health + hpChange) - hpChange);
                connectedTo.monster.baseStats.resources.health = (int)(connectedTo.monster.baseStats.resources.health + hpChange);
                connectedTo.monster.resources.health += mh;

                connectedTo.monster.baseStats.ac = (int)(connectedTo.monster.baseStats.ac + acChange);
                connectedTo.monster.baseStats.ev = (int)(connectedTo.monster.baseStats.ev + evChange);
            }
        }
    }

    //Called whenever a monster gains energy
    /*public override void OnEnergyGained(ref int energy) {}*/

    //Called when a monster recieves an attack type request
    /*public override void OnAttacked(ref int pierce, ref int accuracy) {}*/

    //Called when a monster takes damage from any source, good for making effects fire upon certain types of damage
    /*public override void OnTakeDamage(ref int damage, ref DamageType damageType) {}*/

    //Called when a monster recieves a healing event request
    /*public override void OnHealing(ref int healAmount) {}*/

    //Called whenever a monster takes a step
    /*public override void OnMove(ref Vector2Int newPosition, ref bool isValid) {}*/

    //Called once when a monster hits another monster with a melee attack
    /*public override void OnHitMeleeAttack(ref Monster m) {}*/

    //Called when a monster hits another mosnter with a ranged attack. Called once per hit connectedTo.monster per attack.
    /*public override void OnHitRangedAttack(ref Monster m) {}*/

    //Called whenever a monster deals damage to another monster. Currently doesn't account for status effects!
    /*public override void OnDealDamage(ref Monster m, ref int damage, ref DamageType type) {}*/

    //Called when this monster kills another monster
    /*public override void OnKill(ref Monster m) {}*/

    //Callen when a monster recieves an event with new status effects
    /*public override void OnApplyStatusEffects(ref Effect[] effects) {}*/

    private void LogStatsChange(StatBlock oldStats)
    {
        if (isMultiplier)
        {
            if (oldStats.resources.health * hpChange > oldStats.resources.health) LogChange(true, "Max HP", ((int) (oldStats.resources.health * hpChange)).ToString());
            else if (oldStats.resources.health * hpChange < oldStats.resources.health) LogChange(false, "Max HP", ((int) (oldStats.resources.health * hpChange)).ToString());

            if (oldStats.ac * acChange > oldStats.ac) LogChange(true, "Accuracy", ((int) (oldStats.ac * acChange)).ToString());
            else if (oldStats.ac * acChange < oldStats.ac) LogChange(false, "Accuracy", ((int) (oldStats.ac * acChange)).ToString());

            if (oldStats.ev * evChange > oldStats.ev) LogChange(true, "Evasion", ((int) (oldStats.ev * evChange)).ToString());
            else if (oldStats.ev * evChange < oldStats.ev) LogChange(false, "Evasion", ((int) (oldStats.ev * evChange)).ToString());
        }
        else
        {
            if (oldStats.resources.health + hpChange > 0) LogChange(true, "Max HP", ((int) (oldStats.resources.health + hpChange)).ToString());
            else if (oldStats.resources.health + hpChange < 0) LogChange(false, "Max HP", ((int) (oldStats.resources.health + hpChange)).ToString());

            if (oldStats.ac + acChange > 0) LogChange(true, "Accuracy", ((int) (oldStats.ac + acChange)).ToString());
            else if (oldStats.ac + acChange < 0) LogChange(false, "Accuracy", ((int) (oldStats.ac + acChange)).ToString());

            if (oldStats.ev + evChange > 0) LogChange(true, "Evasion", ((int) (oldStats.ev + evChange)).ToString());
            else if (oldStats.ev + evChange < 0) LogChange(false, "Evasion", ((int) (oldStats.ev + evChange)).ToString());
        }
    }

    private void LogChange(bool isIncrease, string stat, string changeString)
    {
        stat = (isPermanent ? "Base " : "") + $"{stat}";
        // if (isIncrease) LogManager.S.Log($"<color=#FFFF00>{stat}</color> <color=#00FF00>increased to {changeString}</color>!");
        // else LogManager.S.Log($"<color=#FFFF00>{stat}</color> <color=#FF0000>decreased to {changeString}</color>!");
    }
}
