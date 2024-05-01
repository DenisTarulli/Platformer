using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    private void Start()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        currentDestination = pointB.transform;
        yRotation = 90f;
    }

    private void Update()
    {
        VisualFlip();
    }

    private void FixedUpdate()
    {
        EnemyMovement();
    }
}
