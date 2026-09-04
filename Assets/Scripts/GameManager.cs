using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SolTaskType
{
    PlantFood
    // futuras tareas van aquí
}

[System.Serializable]
public class SolTask
{
    public SolTaskType type;
    public int reminderSol;
    public int deadlineSol;
    [TextArea] public string reminderMessage;
    [TextArea] public string failMessage;
    [HideInInspector] public bool reminderShown;
    [HideInInspector] public bool resolved;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int suns = 1;
    public bool plantsCollected;
    public bool plantsPlanted;
    public int plantingSol;
    public float energy = 100f;
    public float maxEnergy = 100f;

    public bool HasEnergy => energy > 0f;

    [Header("Mission")]
    [SerializeField] private int expectedSuns = 10;
    public int ExpectedSuns => expectedSuns;

    public bool MissionCompleted => expectedSuns <= 0;

    private TextMeshProUGUI expectedSunsText;
    private bool gameWon;
    public bool IsGameOver { get; private set; }

    [Header("Sol Tasks")]
    [SerializeField] private List<SolTask> solTasks = new();
    [SerializeField] private float postWakeEventDelay = 1.5f;

    public void ProcessSolTasks()
    {
        foreach (SolTask task in solTasks)
        {
            if (!task.reminderShown && suns >= task.reminderSol)
            {
                task.reminderShown = true;
                EventManager.Instance?.QueueEventDelayed(task.reminderMessage, postWakeEventDelay);
            }

            if (!task.resolved && suns >= task.deadlineSol)
            {
                task.resolved = true;

                if (!IsTaskCompleted(task.type))
                {
                    EventManager.Instance?.QueueEventDelayed(
                        task.failMessage, postWakeEventDelay, () => LoseGame(task.failMessage));
                }
            }
        }
    }

    private bool IsTaskCompleted(SolTaskType type)
    {
        switch (type)
        {
            case SolTaskType.PlantFood:
                return plantsPlanted;
            default:
                return true;
        }
    }

    private void LoseGame(string reason)
    {
        if (IsGameOver || gameWon) return;

        IsGameOver = true;
        Debug.LogWarning($"GAME OVER: {reason}");

        OnGameOverConfirmed();
    }

    private void OnGameOverConfirmed()
    {
        // Aquí cargas la escena de derrota, muestras un menú, etc.
        // SceneManager.LoadScene("GameOverScene");
    }

    public void AdvanceDay()
    {
        if (IsGameOver) return;

        suns++;
        expectedSuns = Mathf.Max(0, expectedSuns - 1);
        UpdateExpectedSunsText();

        if (expectedSuns == 0)
        {
            WinGame();
        }
    }

    public void AdjustExpectedSuns(int amount)
    {
        expectedSuns = Mathf.Max(suns + 1, expectedSuns + amount);
        UpdateExpectedSunsText();

        Debug.Log($"New mission target: Sol {expectedSuns}");
    }

    public void CheckMissionCompletion()
    {
        if (MissionCompleted)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if (gameWon)
            return;

        gameWon = true;
        Debug.LogWarning("GAME WON: all required Sols survived.");
    }

    public float EnergyPercent
    {
        get
        {
            if (maxEnergy <= 0f)
                return 0f;

            return Mathf.Clamp01(energy / maxEnergy);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        energy = Mathf.Clamp(energy, 0f, maxEnergy);
        expectedSuns = Mathf.Max(0, expectedSuns);
    }

    private void Start()
    {
        foreach (TextMeshProUGUI text in Resources.FindObjectsOfTypeAll<TextMeshProUGUI>())
        {
            if (text.gameObject.name != "ExpectedSols")
                continue;

            expectedSunsText = text;
            break;
        }

        UpdateExpectedSunsText();
    }

    private void UpdateExpectedSunsText()
    {
        if (expectedSunsText != null)
        {
            expectedSunsText.text = $"Sols remaining: {expectedSuns}";
        }
    }
}
