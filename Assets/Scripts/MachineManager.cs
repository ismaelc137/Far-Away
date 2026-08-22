using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    public static MachineManager Instance;

    [SerializeField] private List<Machine> machines;

    private void Awake()
    {
        Instance = this;
    }

    public void EndDay()
    {
        foreach (Machine machine in machines)
        {
          machine.EndDay();
        }
    }
}
