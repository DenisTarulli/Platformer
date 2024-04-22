using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float groundDecay = 0.9f;
    [SerializeField] private LayerMask groundMask;

    private bool grounded;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Rigidbody rb;
    private Vector2 moveDirection;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
        grounded = true;
    }

    private void Update()
    {
        Inputs();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        ApplyFriction();
    }

    private void Inputs()
    {
        moveDirection = moveAction.ReadValue<Vector2>();
    }

    private void MovePlayer()
    {     
        if (Mathf.Abs(moveDirection.x) > 0)
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, 0f);
        
        if (Mathf.Abs(moveDirection.y) > 0)
            rb.velocity = new Vector3(rb.velocity.x, moveDirection.y * moveSpeed, 0f);
    }

    private void ApplyFriction()
    {
        if (grounded && moveDirection.x == 0)
            rb.velocity *= groundDecay;
    }
}
