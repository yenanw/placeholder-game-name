using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Interactable
{
    public Tooltip tooltip;
    public Animator chestAnimator;
    public GameObject openingEffect;
    public List<GameObject> powerUps;

    private static readonly int s_open = Animator.StringToHash("OpenChest");
    private static readonly string s_name = "<color=#FFFFFF>Common Chest";

    public void OnCursorEnter()
    {
        tooltip.ShowTooltip(s_name + "\n\n<color=#DDDDDD><size=60%>Press [F] to open");
    }

    public void OnCursorExit()
    {
        tooltip.HideTooltip();
    }

    public void Select()
    {
        chestAnimator.SetTrigger(s_open);

        // set the layer to default so that the selection raytrace doesn't hit it
        gameObject.layer = 0;


        // CLOSE YOUR EYES, DON'T LOOK!
        var childLoot = transform.Find("ChestV1").transform.Find("ChestV1_Lock");
        var spawn = Instantiate(randomPowerUp(), childLoot.position + new Vector3(0.3f, 1f, 0), childLoot.rotation);
        spawn.SetActive(true);
        
        Instantiate(openingEffect, childLoot.position, childLoot.rotation);

        OnCursorExit();
    }

    private GameObject randomPowerUp()
    {
        int index = Random.Range(0, powerUps.Count);
        return powerUps[index];
    }


}
