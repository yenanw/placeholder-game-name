using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickHeal : MonoBehaviour
{
    public PlayerHandler playerHandler;

    public void OnTriggerStay()
    {
        playerHandler.Heal(10 * Time.deltaTime);
    }
}
