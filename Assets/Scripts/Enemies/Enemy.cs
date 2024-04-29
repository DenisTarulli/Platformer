using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotationSmoothingSpeed;
    [SerializeField] protected GameObject pointA;
    [SerializeField] protected GameObject pointB;
    [SerializeField] protected float pointDistanceTurnTrigger;
    [SerializeField] protected Transform enemyVisual;
    protected Vector3 rotation;
    protected float currentHealth;
    protected float yRotation;
    protected Rigidbody rb;
    protected Transform currentDestination;

    public void EnemyTakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    protected virtual void EnemyMovement()
    {
        if (currentDestination == pointB.transform)
            rb.velocity = new Vector3(moveSpeed, 0f, 0f);
        else
            rb.velocity = new Vector3(-moveSpeed, 0f, 0f);

        if (Vector3.Distance(transform.position, currentDestination.position) < pointDistanceTurnTrigger && currentDestination == pointB.transform)
            currentDestination = pointA.transform;

        if (Vector3.Distance(transform.position, currentDestination.position) < pointDistanceTurnTrigger && currentDestination == pointA.transform)
            currentDestination = pointB.transform;
    }

    protected void VisualFlip()
    {
        float targetRotationRight = 90f;
        float targetRotationLeft = -90f;

        if (rb.velocity.x > 0 && (yRotation < targetRotationRight))
        {
            yRotation += Time.deltaTime * rotationSmoothingSpeed;
            if (yRotation > targetRotationRight)
                yRotation = targetRotationRight;
        }
        if (rb.velocity.x < 0 && (yRotation > targetRotationLeft))
        {
            yRotation -= Time.deltaTime * rotationSmoothingSpeed;
            if (yRotation < targetRotationLeft)
                yRotation = targetRotationLeft;
        }

        rotation = new Vector3(0f, yRotation, 0f);

        enemyVisual.localRotation = Quaternion.Euler(rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, pointDistanceTurnTrigger);
        Gizmos.DrawWireSphere(pointB.transform.position, pointDistanceTurnTrigger);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (collision.gameObject.TryGetComponent(out PlayerCombat playerCombat))
            playerCombat.PlayerTakeDamage();
    }
}
