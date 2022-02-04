using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickDamage : MonoBehaviour
{
    public PlayerHandler playerHandler;

    public void OnTriggerStay()
    {
        playerHandler.Damage(10 * Time.deltaTime);
    }
}
