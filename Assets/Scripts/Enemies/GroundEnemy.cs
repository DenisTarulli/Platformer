using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    private void Start()
    {
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
