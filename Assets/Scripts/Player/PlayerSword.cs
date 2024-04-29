using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        if (collision.gameObject.TryGetComponent(out TestEnemy enemy) && playerCombat.attacking)
            enemy.EnemyTakeDamage(playerCombat.attackDamage);
    }
}
