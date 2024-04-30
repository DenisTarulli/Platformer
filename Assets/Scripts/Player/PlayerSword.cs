using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        if (collision.gameObject.TryGetComponent(out GroundEnemy groundEnemy) && playerCombat.attacking)
            groundEnemy.EnemyTakeDamage(playerCombat.attackDamage);

        if (collision.gameObject.TryGetComponent(out FlyingEnemy flyingEnemy) && playerCombat.attacking)
            flyingEnemy.EnemyTakeDamage(playerCombat.attackDamage);
    }
}
