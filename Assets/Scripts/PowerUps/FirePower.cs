using UnityEngine;

public class FirePower : MonoBehaviour, Interactable, ICombatPower
{
    public Tooltip Tooltip;

    private static readonly string s_name = "<color=#005500>Fire Power";
    private static readonly string s_desc = "<color=#FFFFFF><size=80%>Adds fire damage to your weapon.";
    private static readonly string s_redText = "<color=#AA0000><size=50%> I love the sound of them screaming in pain while being burned alive.";

    public void OnCursorEnter()
    {
        Tooltip.ShowTooltip(s_name + "\n" + s_desc + "\n\n" + s_redText + "\n\n<color=#DDDDDD><size=60%>Press [F] to pick up");
    }

    public void OnCursorExit()
    {
        Tooltip.HideTooltip();
    }

    public void Select()
    {
        OnEquip();

        // play particle effect


        // destroy object

        Destroy(gameObject);
    }

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
