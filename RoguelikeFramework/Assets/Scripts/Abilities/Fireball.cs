using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Abilities/Elemental/Fireball", order = 1)]
public class Fireball : Ability
{

    public override void OnCast(Monster caster)
    {
        foreach (Monster m in targeting.affected)
        {
            m.Damage(caster, (int) stats.power, DamageType.CUTTING, DamageSource.ABILITY);
        }
    }
}
