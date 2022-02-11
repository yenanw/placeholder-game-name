using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    // TODO: generalise this
    public Mace equippedWeapon;

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            equippedWeapon.Attack();
        }
    }
}
