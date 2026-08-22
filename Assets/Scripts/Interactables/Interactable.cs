using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;
    public bool playHandAnimation = true;
    [SerializeField]
    protected string promptMessage;
   public virtual string PromptMessage
   {
    get => promptMessage;
    set => promptMessage = value;
   }
    public void baseInteract()
    {
        if(useEvents)
            GetComponent<InteractionEvents>().onInteract.Invoke();
        Interact();
    }
protected virtual void Interact()
    {

    }
}
