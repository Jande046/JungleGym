using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f; //controls how fast character moves


    [Header("Input")]
    private float moveInput;
    private float turnInput;

    private void Start()
    {
       controller.GetComponent<CharacterController>();
    }

    private void Update()
    {
        InputManagement();
        Movement();
    }

    private void GroundMovement()
    {
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move.y = 0; //prevents from moving up and down
        move *= walkSpeed; //modifies movement speed
        controller.Move(move * Time.deltaTime); //framerate indepenedent movement
    }

    private void Movement()
    {
        GroundMovement();
    }
    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

}
