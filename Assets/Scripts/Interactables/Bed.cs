using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Bed : Interactable
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private TextMeshProUGUI sunsText;
    [SerializeField] private Player_Motor motor;
    [SerializeField] private Hands hands;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform sleepTarget;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private Player_Look playerLook;
    private PlayeUI playerUI;
    [SerializeField] private float sleepEnergyThreshold = 30f;
    [SerializeField] private string cannotSleepMessage = "I can still do more.";

    public bool CanSleep =>
    GameManager.Instance != null &&
    GameManager.Instance.energy <= sleepEnergyThreshold;

    public string CannotSleepMessage => cannotSleepMessage;

    private void Start()
    {
        playerLook = FindAnyObjectByType<Player_Look>();
        playerUI = FindAnyObjectByType<PlayeUI>();
    }
    protected override void Interact()
    {
     if (!CanSleep)
     {
        playerUI?.ShowTemporaryMessage(cannotSleepMessage);
        return;
     }

     StartCoroutine(SleepRoutine());
    }

    private IEnumerator SleepRoutine()
    {
        CharacterController controller = motor.GetComponent<CharacterController>();
        controller.enabled = false;
        playerLook.lockLook = true;
        motor.enabled = false;
        hands.HideHands();
        originalCameraPosition = cameraHolder.position;
        originalCameraRotation = cameraHolder.rotation;
        Vector3 startPos = cameraHolder.position;
        Quaternion startRot = cameraHolder.rotation;

        float timer = 0f;
        float duration = 1f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            cameraHolder.position = Vector3.Lerp(
                startPos,
                sleepTarget.position,
                timer / duration
            );

            cameraHolder.rotation = Quaternion.Slerp(
                startRot,
                sleepTarget.rotation,
                timer / duration
            );

            yield return null;
        }

        cameraHolder.position = sleepTarget.position;
        cameraHolder.rotation = sleepTarget.rotation;

        blackScreen.enabled = true;


        yield return new WaitForSeconds(2f);
        GameManager.Instance.AdvanceDay();
        MachineManager.Instance.EndDay();
        RepairManager.Instance.NewDay();
        GameManager.Instance.energy = GameManager.Instance.maxEnergy;
        sunsText.text =
        $"TITAN MISSION\nSOL {GameManager.Instance.suns}";
        sunsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        blackScreen.enabled = false;
        sunsText.gameObject.SetActive(false);
    timer = 0f;

    while (timer < duration)
    {
       timer += Time.deltaTime;

       cameraHolder.position = Vector3.Lerp(
        cameraHolder.position,
        originalCameraPosition,
        timer / duration
      );

       cameraHolder.rotation = Quaternion.Slerp(
        cameraHolder.rotation,
        originalCameraRotation,
        timer / duration
       );

       yield return null;
       }


        cameraHolder.position = originalCameraPosition;
        cameraHolder.rotation = originalCameraRotation;
        hands.ShowHands();
        motor.enabled = true;
        controller.enabled = true;
        playerLook.lockLook = false;
        
        GameManager.Instance.ProcessSolTasks();
        GameManager.Instance.CheckMissionCompletion();
    }
}
