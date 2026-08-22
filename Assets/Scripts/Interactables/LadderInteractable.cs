using UnityEngine;
public class LadderInteractable : Interactable
{
    [SerializeField]
    private LadderController ladderController;
    public Transform topPoint;
    public Transform bottomPoint;
    public GameObject ladderBlocker;
    protected override void Interact()
    {
    ladderController.EnterLadder(this);
    }
}
