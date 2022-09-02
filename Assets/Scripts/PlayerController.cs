using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    // Movement
    [SerializeField] float speed;
    float horInput, verInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 newVelocity = Vector3.zero;

        newVelocity += new Vector3(horInput, rb.velocity.y, verInput);

        rb.velocity = newVelocity.normalized * speed;
    }

    // INPUT
    public void GetMoveInput(InputAction.CallbackContext context)
    {
        horInput = context.ReadValue<Vector2>().x;
        verInput = context.ReadValue<Vector2>().y;
    }
}
