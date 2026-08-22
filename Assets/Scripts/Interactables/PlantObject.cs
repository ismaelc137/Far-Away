using UnityEngine;

public class PlantObject : MonoBehaviour
{
    [SerializeField] private PlantsSO plant;

    private int currentStage = -1;
    private GameObject currentPlant;

    private void Start()
    {
        if (GameManager.Instance.plantsPlanted)
        {
            UpdatePlant();
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.plantsPlanted)
            return;

        UpdatePlant();
    }

    private void UpdatePlant()
    {
        int relativeDays =
            GameManager.Instance.suns -
            GameManager.Instance.plantingSol;

        int desiredStage = relativeDays / 5;

        desiredStage = Mathf.Clamp(
            desiredStage,
            0,
            plant.MaxStage - 1
        );

        if (desiredStage == currentStage)
            return;

        currentStage = desiredStage;

        if (currentPlant != null)
        {
            Destroy(currentPlant);
        }

        currentPlant = Instantiate(
            plant.GetPlantByStage(currentStage),
            transform.position,
            Quaternion.identity,
            transform
        );

        Debug.Log($"Plant stage: {currentStage}");
    }
}