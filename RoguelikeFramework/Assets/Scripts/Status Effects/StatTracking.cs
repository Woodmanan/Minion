using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EffectGroup("Player Specific")]
[CreateAssetMenu(fileName = "New StatTracking", menuName = "Status Effects/StatTracking", order = 1)]
public class StatTracking : Effect
{
    public static int damageTaken;
    public static int damageDealt;
    public static int monstersKilled;
    public static int playerLevel;
    public static int numberMoves;
    public static int turnsTaken;
    public static int stepsTaken;
    public static int floor = 1;
    public static bool victory = false;

    /* The default priority of all functions in this class - the order in which they'll be called
     * relative to other status effects
     * 
     * To override for individual functions, use the [Priority(int)] attribute 
     */
    //public override int priority { get { return 10; } }

    /*
     * These next 3 functions create UI info. Leave GetDisplayInfo() as null to not have a UI piece.
     * Otherwise, return an image and counter (optional) in GetDisplayInfo, and the name and description
     * in the other two functions.
     */

    //Create UI piece
    public override EffectDisplayInfo GetDisplayInfo()
    {
        return null;
    }

    //Returns the name that will be shown on hovering the UI element
    public override string GetDisplayName()
    {
        return "You must override this function, if you can see this.";
    }

    //Returns the description that will be shown on hovering the UI element
    public override string GetDisplayDescription()
    {
        return "You must override this function, if you can see this.";
    }

    //Constuctor for the object; use this in code if you're not using the asset version!
    //Generally nice to include, just for future feature proofing
    public StatTracking()
    {
        //Construct me!

    }

    //Called the moment an effect connects to a monster
    //Use this to apply effects or stats immediately, before the next frame
    public override void OnConnection()
    {
        damageTaken = 0;
        damageDealt = 0;
        monstersKilled = 0;
        playerLevel = 1;
        numberMoves = 0;
        turnsTaken = 0;
        victory = false;
    }

    //Called when an effect gets disconnected from a monster
    /*public override void OnDisconnection() {} */

    //Called at the start of the global turn sequence
    public override void OnTurnStartGlobal()
    {
        turnsTaken++;
    }

    //Called at the end of the global turn sequence
    //public override void OnTurnEndGlobal() {}

    //Called at the start of a monster's turn
    public override void OnTurnStartLocal()
    {
        numberMoves++;
    }

    //Called at the end of a monster's turn
    //public override void OnTurnEndLocal() {}

    //Called whenever a monster takes a step
    public override void OnMove()
    {
        stepsTaken++;
        floor = Map.current.depth;
    }

    //Called whenever a monster returns to full health
    //public override void OnFullyHealed() {}

    //Called when a monster is killed by this unit.
    public override void OnKillMonster(ref Monster monster, ref DamageType type, ref DamageSource source)
    {
        monstersKilled++;
    }

    //Called when the connected monster dies
    //public override void OnDeath() {}

    //Called often, whenever a monster needs up-to-date stats.
    //public override void RegenerateStats(ref StatBlock stats) {}

    //Called wenever a monster gains energy
    //public override void OnEnergyGained(ref int energy) {}

    //Called when a monster gets attacked (REWORKING SOON!)
    //public override void OnAttacked(ref int pierce, ref int accuracy) {}

    //Called by the dealer of damage, when applicable. Modifications here happen before damage is dealt.
    [Priority(300)]
    public override void OnDealDamage(ref int damage, ref DamageType damageType, ref DamageSource source)
    {
        damageDealt += damage;
    }

    //Called when a monster takes damage from any source, good for making effects fire upon certain types of damage
    [Priority(300)]
    public override void OnTakeDamage(ref int damage, ref DamageType damageType, ref DamageSource source)
    {
        damageTaken += damage;
    }

    //Called when a monster recieves a healing event request
    //public override void OnHealing(ref int healAmount) {}

    //Called when new status effects are added. All status effects coming through are bunched together as a list.
    //public override void OnApplyStatusEffects(ref Effect[] effects) {}

    //Called when a spell is cast. Modify spell, or set continue to false in order to cancel the action!
    //public override void OnCastAbility(ref AbilityAction action, ref bool canContinue) {}

    //Called when this monster gains resources. (Different from healing, but can give health)
    //public override void OnGainResources(ref ResourceList resources) {}

    //Called when this monster loses resources. (Different from damage, but can take health)
    //public override void OnLoseResources(ref ResourceList resources) {}

    //Called when this monster gains XP from any source.
    //public override void OnGainXP(ref int XPAmount) {}

    //Called when this monster levels up! Level CANNOT be modified.
    public override void OnLevelUp(ref int Level)
    {
        playerLevel = Level;
    }

    //Called when new status effects are added. All status effects coming through are bunched together as a list.
    //public override void OnRegenerateAbilityStats(ref Targeting targeting, ref AbilityBlock abilityBlock, ref Ability ability) {}

    //Called by spells, in order to determine whether they are allowed to be cast.
    //public override void OnCheckAvailability(ref Ability abilityToCheck, ref bool available) {}

    //Called by spells once targets are selected.
    //public override void OnTargetsSelected(ref Targeting targeting, ref Ability ability) {}

    //Called before spell is cast
    //public override void OnPreCast(ref Ability ability) {}

    //Called after a spell is cast.
    //public override void OnPostCast(ref Ability ability) {}

    //Called when this monster is selected to be hit by a cast. (Right before hit)
    //public override void OnTargetedByAbility(ref AbilityAction action) {}

    //Called after an ability is cast on this monster. (Right after hit)
    //public override void OnHitByAbility(ref AbilityAction action) {}

    //Called when this monster starts an attack action
    //public override void OnStartAttack(ref AttackAction action, ref bool canContinue) {}

    //Called when an attack has collected the weapons that it will use.
    //public override void OnGenerateArmedAttacks(ref List<Weapon> primaryWeapons, ref List<Weapon> secondaryWeapons) {}

    //Called before a primary attack happens
    //public override void OnBeginPrimaryAttack(ref Weapon weapon, ref AttackAction action) {}

    //Called once a primary attack has generated a result. (Before result is used)
    //public override void OnPrimaryAttackResult(ref Weapon weapon, ref AttackAction action, ref AttackResult result) {}

    //Called after an attack has completely finished - results are final
    //public override void OnEndPrimaryAttack(ref Weapon weapon, ref AttackAction action, ref AttackResult result) {}

    //Called before a secondary attack happens
    //public override void OnBeginSecondaryAttack(ref Weapon weapon, ref AttackAction action) {}

    //Called once a primary attack has generated a result. (Before result is used)
    //public override void OnSecondaryAttackResult(ref Weapon weapon, ref AttackAction action, ref AttackResult result) {}

    //Called after a seconary attack has completely finished - results are final
    //public override void OnEndSecondaryAttack(ref Weapon weapon, ref AttackAction action, ref AttackResult result) {}

    //Called when an attack has collected the unarmed slots that it will use.
    //public override void OnGenerateUnarmedAttacks(ref List<EquipmentSlot> slots) {}

    //Called before an unarmed attack begins.
    //public override void OnBeginUnarmedAttack(ref EquipmentSlot slot, ref AttackAction action) {}

    //Called when an unarmed attack has a determined a result, before that result is used.
    //public override void OnUnarmedAttackResult(ref EquipmentSlot slot, ref AttackAction action, ref AttackResult result) {}

    //Called when an unarmed attack has a determined a result, after that result is used.
    //public override void OnEndUnarmedAttack(ref EquipmentSlot slot, ref AttackAction action, ref AttackResult result) {}

    //Called before this monster is hit by a primary attack from another monster.
    //public override void OnBeforePrimaryAttackTarget(ref Weapon weapon, ref AttackAction action, ref AttackResult result) {}

    //Called after this monster is hit by a primary attack from another monster. (Can't modify anymore)
    //public override void OnAfterPrimaryAttackTarget(ref Weapon weapon, ref AttackAction action, ref AttackResult result) {}

    //Called before this monster is hit by a secondary attack from another monster.
    //public override void OnBeforeSecondaryAttackTarget(ref Weapon weapon, ref AttackAction action, ref AttackResult result) {}

    //Called after this monster is hit by a secondary attack from another monster. (Can't modify anymore)
    //public override void OnAfterSecondaryAttackTarget(ref Weapon weapon, ref AttackAction action, ref AttackResult result) {}

    //Called before this monster is hit by an unarmed attack from another monster.
    //public override void OnBeforeUnarmedAttackTarget(ref EquipmentSlot slot, ref AttackAction action, ref AttackResult result) {}

    //Called after this monster is hit by an unarmed attack from another monster. (Can't modify anymore)
    //public override void OnAfterUnarmedAttackTarget(ref EquipmentSlot slot, ref AttackAction action, ref AttackResult result) {}

}
