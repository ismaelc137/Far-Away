using TMPro;
using UnityEngine;

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

    public bool MissionCompleted => suns >= expectedSuns;

    private TextMeshProUGUI expectedSunsText;

    public void AdjustExpectedSuns(int amount)
    {
        expectedSuns = Mathf.Max(suns + 1, expectedSuns + amount);
        UpdateExpectedSunsText();

        Debug.Log($"New mission target: Sol {expectedSuns}");
    }

    public void CheckMissionCompletion()
    {
        if (!MissionCompleted)
            return;

        Debug.Log("MISSION COMPLETE: survival target reached.");
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
        expectedSuns = Mathf.Max(expectedSuns, suns + 1);
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
            expectedSunsText.text = $"Expected Sols: {expectedSuns}";
        }
    }
}
