using UnityEngine;

public class LabPlants : Interactable
{
    public GameObject plantsParent;
    public GameObject emptyPots;

    [SerializeField] 
    private string tooEarlyMessage = "There's nothing to do here yet.";

    private bool canCollect;

    private void OnEnable()
    {
        GameManager.OnSolTaskReminder += HandleSolTaskReminder;
        RefreshAvailability();
    }

    private void OnDisable()
    {
        GameManager.OnSolTaskReminder -= HandleSolTaskReminder;
    }

    private void HandleSolTaskReminder(SolTaskType type)
    {
        if (type == SolTaskType.PlantFood)
            RefreshAvailability();
    }

    private void Update()
    {
        RefreshAvailability();
        promptMessage = GameManager.Instance != null && GameManager.Instance.plantsCollected
            ? "Plants already collected"
            : canCollect ? "Collect the plants" : tooEarlyMessage;
    }

    private void RefreshAvailability()
    {
        canCollect = GameManager.Instance != null &&
            !GameManager.Instance.plantsCollected &&
            GameManager.Instance.HasSolTaskReminder(SolTaskType.PlantFood);
    }

    protected override void Interact()
    {
        RefreshAvailability();
        if (!canCollect || plantsParent == null || emptyPots == null)
            return;

        plantsParent.SetActive(false);
        emptyPots.SetActive(true);
        GameManager.Instance.plantsCollected = true;
        canCollect = false;
    }
}
