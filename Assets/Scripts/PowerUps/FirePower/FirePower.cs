using UnityEngine;

public class FirePower : ICombatPower
{
    private AttackManager _attackManager;
    private GameObject _fireHitEffect;

    public FirePower(GameObject fireHitEffect, AttackManager attackManager)
    {
        this._fireHitEffect = fireHitEffect;
        this._attackManager = attackManager;
    }

    public void OnEquip()
    {
        _attackManager.AddPower(this);
    }

    public float Hit(float damage, Vector3 hitLocation)
    {
        Object.Instantiate(_fireHitEffect, hitLocation, _fireHitEffect.transform.rotation);
        return damage + 10;
    }

    public void OnUnequip()
    {
        _attackManager.RemovePower(this);
    }

}
