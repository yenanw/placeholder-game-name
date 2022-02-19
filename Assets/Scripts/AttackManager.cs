using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: make this class a singleton maybe...
public class AttackManager : MonoBehaviour
{
    public float BaseDamage;

    private HashSet<ICombatPower> _powers = new HashSet<ICombatPower>();

    public void AddPower(ICombatPower power)
    {
        _powers.Add(power);
    }

    public void RemovePower(ICombatPower power) 
    {
        _powers.Remove(power);
    }

    // this is a bit cheesy, but it just creates the effect at where the weapon is...
    public void Hit(IDamageable enemy, Transform weapon) 
    {
        // Vector3 hitLocation = col.ClosestPointOnBounds(weapon.position);
        float finalDmg = BaseDamage;
        foreach (ICombatPower p in _powers)
        {
            finalDmg = p.Hit(finalDmg, weapon.position);
        }

        enemy.Damage(finalDmg);
        Debug.Log("Skeleton got hit by for " + finalDmg + " damage!");
    }
}
