using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float attackAnimDuration;
    public float attackDamage;
    private PlayerInputs playerInputActions;
    private bool attacking = false;

    private void Awake()
    {
        playerInputActions = new PlayerInputs();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Attack.performed += Attack;
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
}
