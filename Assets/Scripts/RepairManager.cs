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

    private void Start()
    {
        // Start with repair work available so the player can spend energy and
        // reach the first rest/day transition without a softlock.
        NewDay();
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
        bool done = machine.AttendedToday;
        string marker = done ? "[X]" : "[ ]";
        string line = $"{marker} {machine.Type}";

        text += done ? $"\n<s>{line}</s>" : $"\n{line}";
     }

      repairText.text = text;
    }
}
