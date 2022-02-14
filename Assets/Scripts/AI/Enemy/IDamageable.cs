public interface IDamageable
{
    float Health { get; set; }

    void Damage(float dmg);

    void Die();
}
