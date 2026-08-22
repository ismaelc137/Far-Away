using UnityEngine;

public class Dome : Interactable
{
    public GameManager gameManager;

    private void Start()
    {
        if (!gameManager.plantsCollected)
        {
            promptMessage = "";
        }
        else
        {
            promptMessage = "Plant samples";
        }
    }

    private void Update()
    {
        if (gameManager.plantsCollected)
        {
            promptMessage = "Plant samples";
        }
        else
        {
            promptMessage = "";
        }
    }

    protected override void Interact()
    {
        if (!gameManager.plantsCollected)
            return;

        gameManager.plantsPlanted = true;
        GameManager.Instance.plantingSol = GameManager.Instance.suns;
        Destroy(gameObject);
    }
}