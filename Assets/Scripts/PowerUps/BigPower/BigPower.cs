using UnityEngine;

public class BigPower : ICombatPower
{
    private AttackManager _attackManager;
    private Transform _weapon;

    public BigPower(Transform weapon, AttackManager attackManager)
    {
        this._attackManager = attackManager;
        this._weapon = weapon;
    }

    public void OnEquip()
    {
        _attackManager.AddPower(this);
        _weapon.localScale += Vector3.one;
    }

    public float Hit(float damage, Vector3 hitLocation)
    {
        return damage + 20;
    }

    public void OnUnequip()
    {
        _attackManager.RemovePower(this);
        _weapon.localScale -= Vector3.one;
    }

}
