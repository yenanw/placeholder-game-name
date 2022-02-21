using UnityEngine;

// i see now that this is copy-paste programming
// TODO: fix this later
public class BigPowerPickUp : MonoBehaviour, Interactable
{
    public Tooltip tooltip;
    public GameObject pickUpEffect;
    public Transform weapon;

    public AttackManager attackManager;

    private BigPower _bigPower;

    private static readonly string s_name = "<color=#005500>Beeg Power";
    private static readonly string s_desc = "<color=#FFFFFF><size=80%>Makes your club big and strong.";
    private static readonly string s_redText = "<color=#AA0000><size=50%> UNGA BUNGA BEEG CLUB";

    private void Awake()
    {
        _bigPower = new BigPower(weapon, attackManager);
    }

    public void OnCursorEnter()
    {
        tooltip.ShowTooltip(s_name + "\n" + s_desc + "\n\n" + s_redText + "\n\n<color=#DDDDDD><size=60%>Press [F] to pick up");
    }

    public void OnCursorExit()
    {
        tooltip.HideTooltip();
    }

    public void Select()
    {
        var effect = Instantiate(pickUpEffect, transform.position, transform.rotation);

        _bigPower.OnEquip();

        OnCursorExit();
        Destroy(gameObject);
    }
}
