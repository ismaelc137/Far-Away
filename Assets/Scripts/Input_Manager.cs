using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Input_Manager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    private Player_Motor motor;
    private Player_Look look;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<Player_Motor>();
        look = GetComponent<Player_Look>();
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.crouch();
        onFoot.Sprint.performed += ctx => motor.SprintStart();
        onFoot.Sprint.canceled += ctx => motor.SprintStop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(InspectionManager.Instance != null &&
        InspectionManager.Instance.IsInspecting)
        {
        return;
     }
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    void LateUpdate()
    {
        if(InspectionManager.Instance != null &&
        InspectionManager.Instance.IsInspecting)
     {
        return;
    }
        look.processLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
