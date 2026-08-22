using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public static Machine Instance;
    [Header("Info")]
    public ComputerType type;

    [Header("State")]
    [Range(0,100)]
   [SerializeField]private bool broken;
   [SerializeField]private float integrity = 100;
   public bool IsBroken => broken;
   [SerializeField] private Transform statusPoint;
   public Transform StatusPoint => statusPoint;
   public float Integrity => integrity;
   private bool processedToday;




    [Header("Repair")]
    public float energyCost = 10;

    [Header("Dependencies")]
    public List<Machine> dependencies = new();
    public ComputerType Type => type;

    public virtual void BreakMachine()
    {
        broken = true;
        processedToday = false;
    }

   public virtual void RepairMachine()
   {
    if (!broken)
        return;
    broken = false;
    processedToday = false;
    
    GameManager.Instance.energy -= energyCost;
    RepairManager.Instance.MachineRepaired(this);
   }

    public virtual void Damage(float amount)
    {
        integrity = Mathf.Max(0, integrity - amount);
    }

    public virtual void Heal(float amount)
    {
        integrity = Mathf.Min(100, integrity + amount);
    }
    public virtual void EndDay()
   {
    if (broken && !processedToday)
    {
        Damage(10);
        processedToday = true;
        foreach (Machine dependency in dependencies)
       {
        dependency.Damage(3);
       }
    }
   }
}