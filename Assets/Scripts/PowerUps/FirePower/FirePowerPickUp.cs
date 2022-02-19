using UnityEngine;

public class FirePowerPickUp : MonoBehaviour, Interactable
{
    public Tooltip tooltip;
    public GameObject pickUpEffect;
    public GameObject fireHitEffect;
    public AttackManager attackManager;

    private FirePower _firePower;

    private static readonly string s_name = "<color=#005500>Fire Power";
    private static readonly string s_desc = "<color=#FFFFFF><size=80%>Adds fire damage to your weapon.";
    private static readonly string s_redText = "<color=#AA0000><size=50%> I love the sound of them screaming in pain while being burned alive.";

    private void Awake()
    {
        _firePower = new FirePower(fireHitEffect, attackManager);
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

        _firePower.OnEquip();

        OnCursorExit();
        Destroy(gameObject);
    }
}
