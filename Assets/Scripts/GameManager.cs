using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SolTaskType
{
    PlantFood,
    Intro
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
    [HideInInspector] public bool reminderActivated;
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
    [SerializeField] private int expectedSuns = 4;
    public int ExpectedSuns => expectedSuns;

    public bool MissionCompleted => expectedSuns <= 0;

    private TextMeshProUGUI expectedSunsText;
    private bool gameWon;
    public bool IsGameOver { get; private set; }
    public bool IsMissionWon => gameWon;

    [Header("Sol Tasks")]
    [SerializeField] private List<SolTask> solTasks = new();
    [SerializeField] private float postWakeEventDelay = 1.5f;

    public static event Action<SolTaskType> OnSolTaskReminder;

    public void ProcessSolTasks()
    {
        foreach (SolTask task in solTasks)
        {
            if (!task.reminderShown && suns >= task.reminderSol)
            {
                task.reminderShown = true;
                if (EventManager.Instance != null)
                {
                    EventManager.Instance.QueueEventDelayed(
                        task.reminderMessage,
                        postWakeEventDelay,
                        onShown: () => ActivateSolTaskReminder(task.type));
                }
                else
                {
                    ActivateSolTaskReminder(task.type);
                }
            }

            if (!task.resolved && task.reminderActivated && suns >= task.deadlineSol)
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

    public bool HasSolTaskReminder(SolTaskType type)
    {
        return solTasks.Exists(task => task.type == type && task.reminderActivated);
    }

    private void ActivateSolTaskReminder(SolTaskType type)
    {
        foreach (SolTask task in solTasks)
        {
            if (task.type == type)
                task.reminderActivated = true;
        }

        OnSolTaskReminder?.Invoke(type);
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
        EventManager.Instance?.QueueEvent(
            $"MISSION FAILED\n\n{reason}\n\nPress CONTINUE to return to the menu.",
            ReturnToMenu);
    }

    public void AdvanceDay()
    {
        if (IsGameOver || gameWon) return;

        suns++;
        expectedSuns = Mathf.Max(0, expectedSuns - 1);
        UpdateExpectedSunsText();

    }

    public void AdjustExpectedSuns(int amount)
    {
        expectedSuns = Mathf.Max(suns + 1, expectedSuns + amount);
        UpdateExpectedSunsText();

        Debug.Log($"New mission target: Sol {expectedSuns}");
    }

    public void CheckMissionCompletion()
    {
        if (!IsGameOver && MissionCompleted)
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
        EventManager.Instance?.QueueEvent(
            "MISSION COMPLETE\n\nYou survived the Titan assignment. This is only the beginning of Far Away.\n\nPress CONTINUE to return to the menu.",
            ReturnToMenu);
    }

    private static void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
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
