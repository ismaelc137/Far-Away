using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Player_Look : MonoBehaviour
{
    public bool lockLook = false;
    public Camera cam;
    private float xrotation = 0f;

    public float xSensitivity = 2f;
    public float ySensitivity = 2f;

private void Start()
{
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}

public void SetSensitivity(float newSensitivity)
{
    xSensitivity = newSensitivity;
    ySensitivity = newSensitivity;
}

    private void Update()
 {
    if (lockLook)
    return;
    if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame &&
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
        // Mouse delta is already measured per frame; scaling by deltaTime makes
        // the look speed depend on frame rate and nearly unusable at low FPS.
        xrotation -= mouseY * ySensitivity * 0.05f;
        xrotation = Mathf.Clamp(xrotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xrotation, 0f, 0f);

        transform.Rotate(Vector3.up * (mouseX * xSensitivity * 0.05f));
    }
    public void ResetLook()
    { 
    xrotation = 0f;
    cam.transform.localRotation = Quaternion.identity;
    }
}
