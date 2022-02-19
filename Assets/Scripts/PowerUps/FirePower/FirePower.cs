using UnityEngine;

public class FirePower : ICombatPower
{

    public void OnEquip()
    {
        Debug.Log("WOOOO POWER UP!");
    }

    public void OnHit()
    {
        throw new System.NotImplementedException();
    }

    public void OnUnequip()
    {
        throw new System.NotImplementedException();
    }


}
