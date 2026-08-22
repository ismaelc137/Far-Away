using TMPro;
using UnityEngine;

public class MachineStatusUI : MonoBehaviour
{
    public static MachineStatusUI Instance;
    [SerializeField] private TMP_Text statusText;
    private Machine currentMachine;
    private Transform followTarget;
   [SerializeField] private float maxDistance = 3f;
   private Transform player;
    private Camera cam;

    private void Awake()
    {
        Instance = this;
        player = Camera.main.transform;
        cam = Camera.main;
        statusText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (followTarget == null)
            return;
       if (Vector3.Distance(player.position, followTarget.position) > maxDistance)
       {
        Hide();
        return;
        }
        Vector3 screenPos = cam.WorldToScreenPoint(followTarget.position);
        statusText.rectTransform.position = screenPos;
    }

    public void Show(Machine machine)
    {
        followTarget = machine.StatusPoint;

        if (currentMachine != machine)
        {
            currentMachine = machine;

            statusText.text =
                $"{machine.Type}\n\n" +
                $"Integrity: {machine.Integrity:0}%\n\n" +
                $"Status: {GetStatus(machine.Integrity)}";
        }

        statusText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        currentMachine = null;
        followTarget = null;

        statusText.gameObject.SetActive(false);
    }

    private string GetStatus(float integrity)
    {
        if (integrity >= 80) return "STABLE";
        if (integrity >= 60) return "DEGRADED";
        if (integrity >= 40) return "CRITICAL";
        if (integrity >= 20) return "FAILURE IMMINENT";

        return "OFFLINE";
    }
}