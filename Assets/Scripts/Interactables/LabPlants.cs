using UnityEngine;

public class LabPlants : Interactable
{
    public GameObject plantsParent;
    public GameObject emptyPots;

    protected override void Interact()
    {
        plantsParent.SetActive(false);
        emptyPots.SetActive(true);
        GameManager.Instance.plantsCollected = true;
    }
}
