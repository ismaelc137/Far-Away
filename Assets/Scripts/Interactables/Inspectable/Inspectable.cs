using UnityEngine;
public class Inspectable : Interactable
{
    [SerializeField]
    [TextArea]
    private string description;

    protected override void Interact()
    {
        InspectionManager.Instance.Inspect(gameObject);
    }

    public string GetDescription()
    {
        return description;
    }
}
