using UnityEngine;
using System.Collections;

public class Computer : Interactable
{
    [SerializeField] private ComputerType computerType;
    public ComputerType Type => computerType;

    [SerializeField] private Player_Motor motor;
    [SerializeField] private Player_Look playerLook;
    [SerializeField] private Hands hands;

    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform computerViewPoint;
    public bool IsBroken => machine.IsBroken;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Machine machine;
   public Machine Machine => machine;

   private void Awake()
   {
    machine = GetComponent<Machine>();
    }
    protected override void Interact()
    {
        if (computerType != ComputerType.Logs &&
            (machine == null || !machine.IsBroken))
        return;

        OpenComputer();
    }

    void OpenComputer()
    {
        originalPosition = cameraHolder.position;
        originalRotation = cameraHolder.rotation;

        ComputerUIManager.Instance.SetCurrentComputer(this);

        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        CharacterController controller = motor.GetComponent<CharacterController>();
        controller.enabled = false;
        playerLook.lockLook = true;
        motor.enabled = false;

        hands.HideHands();

        Vector3 startPos = cameraHolder.position;
        Quaternion startRot = cameraHolder.rotation;

        float timer = 0f;
        float duration = 0.5f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            cameraHolder.position = Vector3.Lerp(
                startPos,
                computerViewPoint.position,
                timer / duration
            );

            cameraHolder.rotation = Quaternion.Slerp(
                startRot,
                computerViewPoint.rotation,
                timer / duration
            );

            yield return null;
        }

        ComputerUIManager.Instance.Open(computerType);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartCloseRoutine()
    {
        StartCoroutine(CloseRoutine());
    }

    private IEnumerator CloseRoutine()
    {
        ComputerUIManager.Instance.Close();

        Vector3 startPos = cameraHolder.position;
        Quaternion startRot = cameraHolder.rotation;

        float timer = 0f;
        float duration = 0.5f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            cameraHolder.position = Vector3.Lerp(
                startPos,
                originalPosition,
                timer / duration
            );

            cameraHolder.rotation = Quaternion.Slerp(
                startRot,
                originalRotation,
                timer / duration
            );

            yield return null;
        }

        cameraHolder.position = originalPosition;
        cameraHolder.rotation = originalRotation;

        hands.ShowHands();

        motor.enabled = true;
        playerLook.lockLook = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CharacterController controller = motor.GetComponent<CharacterController>();
        controller.enabled = true;
    }
    public void Repair()
    {
    machine.RepairMachine();
    }

}
