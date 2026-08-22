using System;
using UnityEngine;

public class Player_Motor : MonoBehaviour
{
    public bool IsMovingOnLadder { get; private set; }
    public bool IsMoving {get; private set; }
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float speed = 7.0f;
    private bool groundedPlayer;
    private float gravity = -4f;
    public float jumpHeight = 3.0f;
    private bool crouching = false;
    private bool lerpCrouch = false;
    private float crouchTimer = 1;
    private bool isClimbing = false;
    public float climbSpeed = 3f;
    public bool IsClimbing
    {
    get => isClimbing;
    set => isClimbing = value;
    } 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 1.4f, p);
            }
         if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }
   public void ProcessMove(Vector2 input)
   {
    if (!controller.enabled)
        return;

    IsMoving = input.magnitude > 0.1f;

    if (isClimbing)
    {
        playerVelocity = Vector3.zero;

        Vector3 climb = Vector3.up * input.y;
        IsMovingOnLadder = Mathf.Abs(input.y) > 0.1f;
        controller.Move(climb * climbSpeed * Time.deltaTime);
        return;
    }

    IsMovingOnLadder = false;

    Vector3 moveDirection = Vector3.zero;
    moveDirection.x = input.x;
    moveDirection.z = input.y;

    controller.Move(
        transform.TransformDirection(moveDirection)
        * speed
        * Time.deltaTime);

    playerVelocity.y += gravity * Time.deltaTime;

    if (groundedPlayer && playerVelocity.y < 0)
    {
        playerVelocity.y = -2f;
    }

    controller.Move(playerVelocity * Time.deltaTime);
    }
    

    public void SprintStart()
    {
    speed = 14f;
    }

    public void SprintStop()
    {
    speed = 7f;
    }
    public void Jump()
    {
        if (crouching)
        {
        crouch();
        return;
        }
        if (groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void crouch()
    { 
    crouching = !crouching;
    crouchTimer = 0;
    lerpCrouch = true;
    }
}
