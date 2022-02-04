using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    public float maxHealth = 100f;

    private float currentHealth;

    public HealthBar healthBar;

    // Start is called before the first frame update
    public void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetUp(maxHealth);
    }

    public void Damage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth < 0)
            currentHealth = 0;

        healthBar.SetHealth(currentHealth);
    }

    public void Heal(float heal)
    {
        currentHealth += heal;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        healthBar.SetHealth(currentHealth);
    }
}
