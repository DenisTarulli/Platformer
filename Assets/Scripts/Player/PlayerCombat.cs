using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float invulnerabilityTime;
    private int currentHealth;
    public float attackDamage;
    [HideInInspector] public bool attacking = false;
    private bool invulnerable = false;

    [Header("Animations")]
    [SerializeField] private Animator anim;
    [SerializeField] private float attackAnimDuration;

    [Header("Inputs")]
    private PlayerInputs playerInputActions;

    [Header("UI")]
    private UpdateUI updateUI;
    private PauseMenu pauseMenu;

    private void Awake()
    {
        playerInputActions = new PlayerInputs();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Attack.performed += Attack;
    }

    private void Start()
    {
        updateUI = FindObjectOfType<UpdateUI>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        currentHealth = maxHealth;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !attacking && !pauseMenu.gameIsPaused)
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
        if (invulnerable) return;

        currentHealth--;
        updateUI.HealthUpdate(currentHealth);

        StartCoroutine(nameof(Invulnerability));
        Debug.Log(currentHealth);
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;

        yield return new WaitForSeconds(invulnerabilityTime);

        invulnerable = false;
    }
}
