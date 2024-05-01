using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float invulnerabilityTime;
    [HideInInspector] public int currentHealth;
    public float attackDamage;
    [HideInInspector] public bool attacking = false;
    private bool invulnerable = false;

    [Header("Animations")]
    [SerializeField] private Animator anim;
    [SerializeField] private float attackAnimDuration;
    [SerializeField] private ParticleSystem swordTrail;

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

    private void Update()
    {
        FallCheck();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !attacking && !pauseMenu.gameIsPaused && !GameManager.Instance.gameIsOver)
        {
            StartCoroutine(nameof(AttackAction));
        }
    }

    private IEnumerator AttackAction()
    {
        anim.SetBool("isAttacking", true);
        attacking = true;
        swordTrail.Play();

        yield return new WaitForSeconds(attackAnimDuration);

        anim.SetBool("isAttacking", false);
        attacking = false;
    }

    public void PlayerTakeDamage()
    {
        if (invulnerable) return;

        currentHealth--;
        updateUI.HealthUpdate(currentHealth);
        if (currentHealth <= 0)
            GameManager.Instance.GameOver();
        else
            StartCoroutine(nameof(Invulnerability));
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;

        yield return new WaitForSeconds(invulnerabilityTime);

        invulnerable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag.ToString())
        {
            case "Gem":
                other.GetComponentInParent<Gem>().Collect();
                break;
            case "pwp_Attack":
                other.GetComponentInParent<AttackPowerUp>().PickPwp();
                break;
            case "Goal":
                GameManager.Instance.GameOver();
                break;
        }
    }
    private void FallCheck()
    {
        if (transform.position.y < -5f)
        {
            currentHealth = 0;
            GameManager.Instance.GameOver();
        }
    }
}
