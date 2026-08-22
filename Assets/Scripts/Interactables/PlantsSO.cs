using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlantsSO", menuName = "Scriptable Objects/PlantsSO")]
public class PlantsSO : ScriptableObject
{
    public string plantName;

    public List<GameObject> plantPrefabs;

    public int MaxStage
    {
        get { return plantPrefabs.Count; }
    }

    public GameObject GetPlantByStage(int stage)
    {
        if (stage < 0 || stage >= MaxStage)
            return null;

        return plantPrefabs[stage];
    }
}