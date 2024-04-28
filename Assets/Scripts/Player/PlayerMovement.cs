using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpSpeed;
    private Rigidbody rb;

    [Header("Drag")]
    [SerializeField] private float groundDecay;
    [SerializeField] private float gravityMultiplier;

    [Header("Ground check")]
    [SerializeField] private LayerMask groundMask;
    private bool grounded;
    private CapsuleCollider capsuleCollider;

    [Header("Rotation")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private float rotationSmoothingSpeed;
    private Vector3 rotation;
    private float yRotation;

    [Header("Animations")]
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem stepParticles;
    [SerializeField] private float jumpToFallAnimSmoothing;

    [Header("Inputs")]
    PlayerInputs playerInputActions;
    private Vector2 inputVector;

    private void Awake()
    {
        playerInputActions = new PlayerInputs();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
    }

    private void Start()
    {
        GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        yRotation = -90f;
    }

    private void Update()
    {
        Inputs();
        Particles();
        VisualRotation();
        Animations();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        Movement();
        ApplyFriction();
        GravityCompensation();
    }

    private void Inputs()
    {
        inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
    }

    private void Movement()
    {
        if (Mathf.Abs(inputVector.x) > 0)
        {
            float increment = inputVector.x * acceleration;
            float newSpeed = Mathf.Clamp(rb.velocity.x + increment, -maxSpeed, maxSpeed);
            rb.velocity = new Vector3(newSpeed, rb.velocity.y, 0f);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

    private void ApplyFriction()
    {
        Vector3 vel = rb.velocity;

        if (grounded && inputVector.x == 0)
            rb.velocity *= groundDecay;

        if (!grounded && inputVector.x == 0)
        {
            vel.x *= groundDecay;
            rb.velocity = vel;
        }
    }

    private void GroundCheck()
    {
        float extraHeight = 0.15f;
        grounded = Physics.Raycast(capsuleCollider.bounds.center, Vector3.down, capsuleCollider.bounds.extents.y + extraHeight, groundMask);        
    }

    private void GravityCompensation()
    {
        if (!grounded)
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Force);
    }

    private void Particles()
    {
        Vector3 vel = rb.velocity;

        if (grounded && Mathf.Abs(vel.x) == maxSpeed)
            stepParticles.Play();
    }

    private void VisualRotation()
    {
        float targetRotationRight = -90f;
        float targetRotationLeft = 90f;

        if (inputVector.x > 0 && (yRotation > targetRotationRight))
        {
            yRotation -= Time.deltaTime * rotationSmoothingSpeed;
            if (yRotation < targetRotationRight)
                yRotation = targetRotationRight;
        }
        if (inputVector.x < 0 && (yRotation < targetRotationLeft))
        {
            yRotation += Time.deltaTime * rotationSmoothingSpeed;
            if (yRotation > targetRotationLeft)
                yRotation = targetRotationLeft;
        }

        rotation = new Vector3(0f, yRotation, 0f);

        visualTransform.localRotation = Quaternion.Euler(rotation);
    }

    private void Animations()
    {
        anim.SetFloat("horizontal", Mathf.Abs(rb.velocity.x) / maxSpeed);
        anim.SetFloat("vertical", rb.velocity.y / jumpToFallAnimSmoothing);
        anim.SetBool("isJumping", !grounded);
    }
}
