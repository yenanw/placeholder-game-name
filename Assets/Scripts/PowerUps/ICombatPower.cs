using UnityEngine;

public interface ICombatPower : IPower
{
    float Hit(float damage, Vector3 hitLocation);
}
