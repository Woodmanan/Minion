using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Debug_Kill_All", menuName = "Abilities/Debug_Kill_All", order = 1)]
public class Debug_Kill_All : Ability
{
    public float percentOfHealth;
	//Check activation, but for requirements that you are willing to override (IE, needs some amount of gold to cast)
    public override bool OnCheckActivationSoft(Monster caster)
    {
        return true;
    }

    //Check activation, but for requirements that MUST be present for the spell to launch correctly. (Status effects will never override)
    public override bool OnCheckActivationHard(Monster caster)
    {
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        return true;
        #else
        return false;
        #endif
    }

    public override void OnCast(Monster caster)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        foreach (Monster m in targeting.affected)
        {
            m.Damage((int)(m.stats.resources.health * percentOfHealth / 100), DamageType.NONE, DamageSource.ABILITY, "DEBUG: Insta-kill of " + m.name);
        }
#endif
    }
}
