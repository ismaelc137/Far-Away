using UnityEngine;
using System.Collections;

public class Player_interact : MonoBehaviour
{
    [SerializeField] private Animator handAnimator;
    [SerializeField] private float interactDelay = 0.15f;

    private bool isInteracting;

    private Camera cam;

    [SerializeField]
    private float distance = 3f;

    [SerializeField]
    private LayerMask mask;

    private PlayeUI PlayerUI;
    private Input_Manager inputManager;

    void Start()
    {
        cam = GetComponent<Player_Look>().cam;
        PlayerUI = GetComponent<PlayeUI>();
        inputManager = GetComponent<Input_Manager>();
    }

    void Update()
    {
        if (InspectionManager.Instance != null &&
            InspectionManager.Instance.IsInspecting)
        {
            PlayerUI.UpdateText(string.Empty);
            return;
        }

        PlayerUI.UpdateText(string.Empty);

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            Machine machine = hitInfo.collider.GetComponent<Machine>();
            if (machine != null)
               MachineStatusUI.Instance.Show(machine);
           else
              MachineStatusUI.Instance.Hide();
          Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                PlayerUI.UpdateText(interactable.PromptMessage);

                if (!CanInteractWith(interactable))
                {
                    PlayerUI.ShowNoEnergyMessage();
                    return;
                }

                if (inputManager.onFoot.Interact.triggered && !isInteracting)
                {
                    if (interactable.playHandAnimation)
                    {
                        StartCoroutine(InteractRoutine(interactable));
                    }
                    else
                    {
                        interactable.baseInteract();
                    }
                }
            }
        }
    }

    private bool CanInteractWith(Interactable interactable)
    {
        if (GameManager.Instance == null || GameManager.Instance.HasEnergy)
            return true;

        return interactable is Bed;
    }

    private IEnumerator InteractRoutine(Interactable interactable)
    {
        isInteracting = true;

        handAnimator.SetTrigger("Interact");

        yield return new WaitForSeconds(interactDelay);

        interactable.baseInteract();

        yield return new WaitForSeconds(0.3f);

        isInteracting = false;
    }

}
