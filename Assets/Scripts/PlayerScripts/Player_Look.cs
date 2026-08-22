using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Player_Look : MonoBehaviour
{
    public bool lockLook = false;
    public Camera cam;
    private float xrotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
 {
    if (lockLook)
    return;
    if (Keyboard.current.escapeKey.wasPressedThisFrame)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    if (Mouse.current.leftButton.wasPressedThisFrame &&
        Cursor.lockState == CursorLockMode.None)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }   
 }

    public void processLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
     if (lockLook)
     return;
        xrotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xrotation = Mathf.Clamp(xrotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xrotation, 0f, 0f);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
    public void ResetLook()
    { 
    xrotation = 0f;
    cam.transform.localRotation = Quaternion.identity;
    }
}