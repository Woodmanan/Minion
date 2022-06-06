using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AreaDamage", menuName = "Abilities/AreaDamage", order = 1)]
public class AreaDamage : Ability
{
    public bool damagesAllies = false;
    public DamageType type;
    public Roll damage;
	//Check activation, but for requirements that you are willing to override (IE, needs some amount of gold to cast)
    public override bool OnCheckActivationSoft(Monster caster)
    {
        return true;
    }

    //Check activation, but for requirements that MUST be present for the spell to launch correctly. (Status effects will never override)
    public override bool OnCheckActivationHard(Monster caster)
    {
        return true;
    }

    public override void OnCast(Monster caster)
    {
        List<Monster> targets = targeting.affected;
        if (!damagesAllies)
        {
            targets = targets.FindAll(x => x.IsEnemy(caster));
        }

        foreach (Monster m in targets)
        {
            m.Damage(caster, damage.evaluate() + (int) stats.power, type, DamageSource.ABILITY);
        }
    }
}
