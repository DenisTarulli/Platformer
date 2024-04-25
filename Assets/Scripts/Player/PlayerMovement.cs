using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpInput;
    [SerializeField] private float groundDecay;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float rotationSmoothingSpeed;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private ParticleSystem stepParticles;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform visualTransform;

    private CapsuleCollider capsuleCollider;
    private bool grounded;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody rb;
    private Vector2 moveDirection;
    private Vector3 rotation;
    private float yRotation = -90f;
    

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        Inputs();
        GroundCheck();
        Particles();
        VisualRotation();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        ApplyFriction();
        GravityCompensation();
    }

    private void Inputs()
    {
        moveDirection = moveAction.ReadValue<Vector2>();
        jumpInput = jumpAction.ReadValue<float>();
    }

    private void MovePlayer()
    {     
        if (Mathf.Abs(moveDirection.x) > 0)
        {
            float increment = moveDirection.x * acceleration;
            float newSpeed = Mathf.Clamp(rb.velocity.x + increment, -maxSpeed, maxSpeed);
            rb.velocity = new Vector3(newSpeed, rb.velocity.y, 0f);              
        }
        
        if (jumpInput > 0 && grounded)
            rb.AddForce (Vector3.up * jumpSpeed, ForceMode.Impulse);

        anim.SetFloat("horizontal", Mathf.Abs(rb.velocity.x) / maxSpeed);
    }

    private void ApplyFriction()
    {
        Vector3 vel = rb.velocity;

        if (grounded && moveDirection.x == 0)
            rb.velocity *= groundDecay;

        if (!grounded && moveDirection.x == 0)
        {
            vel.x *= groundDecay;
            rb.velocity = vel;
        }
    }

    private void GroundCheck()
    {
        float extraHeight = 0.05f;
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

        if (moveDirection.x > 0 && (yRotation > targetRotationRight))
        {
            yRotation -= Time.deltaTime * rotationSmoothingSpeed;
            if (yRotation < targetRotationRight)
                yRotation = targetRotationRight;
        }
        if (moveDirection.x < 0 && (yRotation < targetRotationLeft))
        {
            yRotation += Time.deltaTime * rotationSmoothingSpeed;
            if (yRotation > targetRotationLeft)
                yRotation = targetRotationLeft;
        }

        rotation = new Vector3(0f, yRotation, 0f);

        visualTransform.localRotation = Quaternion.Euler(rotation);
    }
}
