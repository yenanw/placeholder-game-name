using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MonoBehaviour
{
    public Animator animator;
    public int Damage;

    private HashSet<IDamageable> _hitEnemies = new HashSet<IDamageable>();

    private static readonly int s_attack = Animator.StringToHash("Base_Attack");

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            var enem = col.GetComponent<IDamageable>();
            if (_hitEnemies.Contains(enem))
                return;

            enem.Damage(Damage);
            _hitEnemies.Add(enem);
        }
    }

    public void Attack()
    {
        StartCoroutine(WaitForAttackFinish());
    }

    public void ResetAttacked() => _hitEnemies.Clear();

    private IEnumerator WaitForAttackFinish()
    {
        animator.SetTrigger(s_attack);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);

        ResetAttacked();
    }
}
