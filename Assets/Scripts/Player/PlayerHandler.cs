using UnityEngine;

public class PlayerHandler : MonoBehaviour, IDamageable
{
    public HealthBar healthBar;

    public float MaxHealth = 100f;
    public float Health { get; set; }

    // Start is called before the first frame update
    public void Start()
    {
        Health = MaxHealth;
        healthBar.SetUp(MaxHealth);
    }

    public void Damage(float dmg)
    {
        Health -= dmg;

        if (Health < 0)
        {
            Health = 0;
            Die();
        }

        healthBar.SetHealth(Health);
    }

    public void Heal(float heal)
    {
        Health += heal;

        if (Health > MaxHealth)
            Health = MaxHealth;

        healthBar.SetHealth(Health);
    }

    public void Die()
    {
        Debug.Log("You thought I was dead but you just activated my trap card!");
        Heal(100);
    }
}
