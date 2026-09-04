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
   [SerializeField] private bool attendedToday;
   public bool IsBroken => broken;
   public bool AttendedToday => attendedToday;
   [SerializeField] private Transform statusPoint;
   public Transform StatusPoint => statusPoint;
   public float Integrity => integrity;
   public string RepairStatus
   {
       get
       {
           if (!broken) return "REPAIRED";
           if (integrity >= mediumIntegrity) return "MEDIUM REPAIR";
           if (integrity >= patchIntegrity) return "PATCHED";
           return "UNATTENDED";
       }
   }
   private bool processedToday;




    [Header("Repair")]
    [SerializeField, Min(0f)] private float patchIntegrity = 30f;
    [SerializeField, Min(0f)] private float patchEnergyCost = 15f;
    [SerializeField, Min(0f)] private float mediumIntegrity = 65f;
    [SerializeField, Min(0f)] private float mediumEnergyCost = 30f;
    [SerializeField, Min(0f)] private float totalEnergyCost = 60f;

    [Header("Dependencies")]
    public List<Machine> dependencies = new();
    public ComputerType Type => type;

    public virtual void BreakMachine()
    {
    broken = true;
    integrity = Mathf.Max(0f, integrity - Random.Range(30f, 90f));
    processedToday = false;
    attendedToday = false;
    }

   public virtual bool ApplyPatch()
   {
       return RepairTo(patchIntegrity, patchEnergyCost, false);
   }

   public virtual bool ApplyMediumRepair()
   {
       return RepairTo(mediumIntegrity, mediumEnergyCost, false);
   }

   public virtual bool ApplyTotalRepair()
   {
       return RepairTo(100f, totalEnergyCost, true);
   }

   public virtual void RepairMachine()
   {
       ApplyTotalRepair();
   }

   private bool RepairTo(float targetIntegrity, float energyCost, bool completelyFixed)
   {
       if (!broken || GameManager.Instance == null || GameManager.Instance.energy < energyCost)
           return false;
       integrity = completelyFixed
       ? 100f
       : Mathf.Min(100f, integrity + targetIntegrity);
       GameManager.Instance.energy = Mathf.Max(0f, GameManager.Instance.energy - energyCost);
       attendedToday = true;
 
       if (completelyFixed)
       {
           broken = false;
           processedToday = false;
       }

       RepairManager.Instance?.MachineRepaired(this);
       return true;
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
        float overnightDamage = integrity >= mediumIntegrity ? 2f : integrity >= patchIntegrity ? 5f : 10f;
        Damage(overnightDamage);
        processedToday = true;
        foreach (Machine dependency in dependencies)
       {
        dependency.Damage(3);
       }
    }
   }
}