using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ComputerUIManager : MonoBehaviour
{
    public static ComputerUIManager Instance;

    [SerializeField] private GameObject rootCanvas;

    [SerializeField] private GameObject logsPanel;
    [SerializeField] private GameObject heatingPanel;
    [SerializeField] private GameObject waterPanel;
    [SerializeField] private GameObject communicationsPanel;
    [SerializeField] private GameObject powerPanel;
    [SerializeField] private GameObject pilarPanel;
    [SerializeField] private GameObject labPanel;
    [SerializeField] private GameObject pressurePanel;
    [SerializeField] private Player_Motor motor;
    [SerializeField] private Player_Look playerLook;
    [SerializeField] private Hands hands;
    private Computer currentComputer;
    private Button patchButton;
    private Button mediumRepairButton;
    private Button totalRepairButton;

    public void SetCurrentComputer(Computer computer)
    {
        currentComputer = computer;
    }

    private void Awake()
    {
        Instance = this;
        CreateRepairButtons();
    }

    public void Open(ComputerType type)
    {
        rootCanvas.SetActive(true);
        HideAllPanels();
        SetRepairButtonsVisible(type != ComputerType.Logs);

        switch (type)
        {
            case ComputerType.Logs:
                logsPanel.SetActive(true);
                break;

            case ComputerType.Heating:
                heatingPanel.SetActive(true);
                break;

            case ComputerType.Water:
                waterPanel.SetActive(true);
                break;

            case ComputerType.Communications:
                communicationsPanel.SetActive(true);
                break;

            case ComputerType.Power:
                powerPanel.SetActive(true);
                break;

            case ComputerType.Pilar:
                pilarPanel.SetActive(true);
                break;

            case ComputerType.Pressure:
                pressurePanel.SetActive(true);
                break;

            case ComputerType.Lab:
                labPanel.SetActive(true);
                break;
        }
    }

    private void HideAllPanels()
    {
        logsPanel.SetActive(false);
        heatingPanel.SetActive(false);
        waterPanel.SetActive(false);
        communicationsPanel.SetActive(false);
        pilarPanel.SetActive(false);
        pressurePanel.SetActive(false);
        powerPanel.SetActive(false);
        labPanel.SetActive(false);
    }

    private void SetRepairButtonsVisible(bool visible)
    {
        if (patchButton != null)
            patchButton.gameObject.SetActive(visible);
        if (mediumRepairButton != null)
            mediumRepairButton.gameObject.SetActive(visible);
        if (totalRepairButton != null)
            totalRepairButton.gameObject.SetActive(visible);
    }

    public void Close()
    {
        rootCanvas.SetActive(false);
    }

    public void CloseComputer()
    {
        if (currentComputer != null)
        {
            currentComputer.StartCloseRoutine();
        }
    }

    public void RepairCurrentComputer()
    {
        RepairCurrentComputer(MachineRepairType.Total);
    }

    public void PatchCurrentComputer()
    {
        RepairCurrentComputer(MachineRepairType.Patch);
    }

    public void MediumRepairCurrentComputer()
    {
        RepairCurrentComputer(MachineRepairType.Medium);
    }

    public void TotalRepairCurrentComputer()
    {
        RepairCurrentComputer(MachineRepairType.Total);
    }

    private void RepairCurrentComputer(MachineRepairType repairType)
    {
        if (currentComputer == null || currentComputer.Type == ComputerType.Logs ||
            currentComputer.Machine == null || !currentComputer.Machine.IsBroken)
            return;

        bool repaired = repairType switch
        {
            MachineRepairType.Patch => currentComputer.Machine.ApplyPatch(),
            MachineRepairType.Medium => currentComputer.Machine.ApplyMediumRepair(),
            _ => currentComputer.Machine.ApplyTotalRepair()
        };

        if (repaired)
            currentComputer.StartCloseRoutine();
    }

    private void CreateRepairButtons()
    {
        Button repairTemplate = FindLegacyRepairButton();
        if (repairTemplate == null)
        {
            Debug.LogWarning("Repair button template was not found.");
            return;
        }

        patchButton = Instantiate(repairTemplate, repairTemplate.transform.parent);
        mediumRepairButton = Instantiate(repairTemplate, repairTemplate.transform.parent);
        totalRepairButton = repairTemplate;

        ConfigureRepairButton(patchButton, "PATCH  |  30%  |  5 ENERGY", new Vector2(-180f, 60f), PatchCurrentComputer);
        ConfigureRepairButton(mediumRepairButton, "MEDIUM  |  65%  |  15 ENERGY", new Vector2(0f, 60f), MediumRepairCurrentComputer);
        ConfigureRepairButton(totalRepairButton, "TOTAL  |  100%  |  30 ENERGY", new Vector2(180f, 60f), TotalRepairCurrentComputer);
    }

    private Button FindLegacyRepairButton()
    {
        foreach (Button button in rootCanvas.GetComponentsInChildren<Button>(true))
        {
            for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
            {
                if (button.onClick.GetPersistentMethodName(i) == nameof(RepairCurrentComputer))
                    return button;
            }
        }

        return null;
    }

    private void ConfigureRepairButton(Button button, string label, Vector2 position, UnityAction action)
    {
        button.name = label;
        button.GetComponent<RectTransform>().anchoredPosition = position;
        button.onClick.RemoveAllListeners();

        for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
            button.onClick.SetPersistentListenerState(i, UnityEventCallState.Off);

        button.onClick.AddListener(action);
        TMP_Text text = button.GetComponentInChildren<TMP_Text>(true);
        if (text != null)
            text.text = label;
    }

    private enum MachineRepairType
    {
        Patch,
        Medium,
        Total
    }
}
