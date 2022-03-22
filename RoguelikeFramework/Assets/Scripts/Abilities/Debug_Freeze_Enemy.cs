using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Debug_Freeze_Enemy", menuName = "Abilities/Debug_Freeze_Enemy", order = 1)]
public class Debug_Freeze_Enemy : Ability
{
    public int energyVal;
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
        foreach (Monster m in targeting.affected)
        {
            m.energy = energyVal;
        }
    }
}
