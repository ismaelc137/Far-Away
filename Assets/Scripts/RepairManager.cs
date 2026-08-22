using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RepairManager : MonoBehaviour
{
    public static RepairManager Instance;

    [SerializeField] private List<Machine> machines;
    [SerializeField] private TMP_Text repairText;

    private List<Machine> brokenMachines = new();

    private void Awake()
    {
        Instance = this;
    }

    public void NewDay()
    {
        brokenMachines.Clear();
        

        List<Machine> available = new(machines);
        available.RemoveAll(machine =>
            machine == null || machine.Type == ComputerType.Logs);

        for (int i = 0; i < 3 && available.Count > 0; i++)
        {
            int index = Random.Range(0, available.Count);

            Machine machine = available[index];

            machine.BreakMachine();

            brokenMachines.Add(machine);

            available.RemoveAt(index);
        }

        UpdateUI();
    }

    public void MachineRepaired(Machine machine)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        string text = "MISSION CONTROL\n\nREPAIR REQUESTS\n\n";

        foreach (Machine machine in brokenMachines)
        {
            if (machine.IsBroken)
                text += "[ ] ";
            else
                text += "[/] ";

            text += machine.Type + "\n";
        }

        repairText.text = text;
    }
}
