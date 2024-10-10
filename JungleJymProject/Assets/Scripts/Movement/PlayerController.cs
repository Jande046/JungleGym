using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    [SerializeField] private Transform camera; //references the camera

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f; //controls how fast character moves
    [SerializeField] private float sprintSpeed = 10f; //doubles walkSpeed
    [SerializeField] private float sprintTransitSpeed =5f; //how fast the player transit between normal and sprint speed

    [SerializeField] private float turningSpeed = 2f; //controls how fast player turns to align with the camera
    [SerializeField] private float gravity = 9.81f; //gravity
    [SerializeField] private float jumpHeight = 2f;
    private float verticalVelocity;
    private float speed;

    [Header("Input")]
    private float moveInput;
    private float turnInput;

    private void Start()
    {
       controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        InputManagement();
        Movement();
        
    }

    private void GroundMovement()
    {
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move = camera.transform.TransformDirection(move);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = Mathf.Lerp(speed, sprintSpeed, sprintTransitSpeed * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, walkSpeed, sprintTransitSpeed * Time.deltaTime);
        }

        move *= speed; //modifies movement speed
        move.y = VerticalForceCalculation();
        controller.Move(move * Time.deltaTime); //framerate indepenedent movement
        
    }

    private void Movement()
    {
        GroundMovement();
        Turn();
    }

    private void Turn()
    {
        if (Math.Abs(turnInput) > 0 || Math.Abs(moveInput) > 0)
        {
            Vector3 currentLookDirection = controller.velocity.normalized;
            currentLookDirection.y = 0; //keeps rotation horizontal
            currentLookDirection.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(currentLookDirection); //aligns with current rotation of camera
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turningSpeed); //smooth turning

        }
        
    }

    private float VerticalForceCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
            if(Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2); //calculates initial velocity to reach jump height factoring gravity pull.
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }
    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

}
