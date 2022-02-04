public class HealthSystem
{
    private float health;
    private readonly float maxHealth;

    public HealthSystem(float maxHealth)
    {
        this.health = maxHealth;
        this.maxHealth = maxHealth;
    }

    public void Damage(float dmg)
    {
        health -= dmg;
        // health cannot be negative
        if (health < 0)
        {
            health = 0;
        }
    }

    public void Heal(float heal)
    {
        health += heal;
        // health cannot be more than max
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public float Health()
    {
        return health;
    }
}
