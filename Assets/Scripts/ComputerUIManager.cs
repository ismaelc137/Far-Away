using UnityEngine;

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

    public void SetCurrentComputer(Computer computer)
    {
        currentComputer = computer;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Open(ComputerType type)
    {
        rootCanvas.SetActive(true);
        HideAllPanels();

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
        if (currentComputer == null)
            return;

        if (currentComputer.Type != ComputerType.Logs &&
            currentComputer.Machine != null &&
            currentComputer.Machine.IsBroken)
        {
            currentComputer.Machine.RepairMachine();
        }

        currentComputer.StartCloseRoutine();
    }
}
