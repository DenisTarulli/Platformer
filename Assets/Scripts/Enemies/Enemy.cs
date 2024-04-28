using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    protected float currentHealth;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (collision.gameObject.TryGetComponent(out PlayerCombat playerCombat))
            playerCombat.PlayerTakeDamage();
    }
}
