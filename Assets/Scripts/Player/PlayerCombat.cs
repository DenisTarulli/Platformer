using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth;
    private int currentHealth;
    public float attackDamage;
    private bool attacking = false;

    [Header("Animations")]
    [SerializeField] private Animator anim;
    [SerializeField] private float attackAnimDuration;

    [Header("Inputs")]
    private PlayerInputs playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputs();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Attack.performed += Attack;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !attacking)
        {
            StartCoroutine(nameof(AttackAction));
        }
    }

    private IEnumerator AttackAction()
    {
        anim.SetBool("isAttacking", true);
        attacking = true;

        yield return new WaitForSeconds(attackAnimDuration);

        anim.SetBool("isAttacking", false);
        attacking = false;
    }

    public void PlayerTakeDamage()
    {
        currentHealth--;
        Debug.Log(currentHealth);
    }
}
