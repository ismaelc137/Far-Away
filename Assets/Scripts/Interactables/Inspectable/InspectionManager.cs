using UnityEngine;
using UnityEngine.InputSystem;

public class InspectionManager : MonoBehaviour
{
    private Hands hands;
    private float inspectDistance = 0.75f;
    private float rotationX;
    private float rotationY;
    private PlayeUI playerUI;
    public bool IsInspecting => isInspecting;
    private Input_Manager inputManager;
    private GameObject currentObject;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isInspecting;
    public static InspectionManager Instance;
    [SerializeField]
    private Transform inspectPoint;
    //mmm
    [System.Obsolete]
    private void Awake()
    {
        hands = FindFirstObjectByType<Hands>();
        playerUI = FindFirstObjectByType<PlayeUI>();
        Instance = this;
        inputManager = FindFirstObjectByType<Input_Manager>();
    }
   private void Update()
 {
    float scroll =
    Mouse.current.scroll.ReadValue().y;
    if(isInspecting &&
       inputManager.onFoot.ExitInspect.triggered)
    {
        StopInspecting();
    }
   if(isInspecting)
   {
    hands.HideHands();
    inspectDistance += scroll * 0.1f;
    inspectDistance =
    Mathf.Clamp(inspectDistance, 0.1f, 1f);
    currentObject.transform.position =
    inspectPoint.position +
    inspectPoint.forward * inspectDistance;
    Vector2 mouseDelta = Mouse.current.delta.ReadValue();

    rotationY -= mouseDelta.x * 0.2f;
    rotationX += mouseDelta.y * 0.2f;

    currentObject.transform.rotation =
        inspectPoint.rotation *
        Quaternion.Euler(rotationX, rotationY, 0);
   }
 }
    private void StopInspecting()
   {
    hands.ShowHands();
    currentObject.transform.position = originalPosition;
    currentObject.transform.rotation = originalRotation;

    currentObject = null;

    isInspecting = false;
    playerUI.HideDescription();
   }
 public void Inspect(GameObject obj)
   {
    inspectDistance = 0.75f;
    rotationX = 0f;
    rotationY = 0f;
    Inspectable inspectable =
    obj.GetComponent<Inspectable>();

    if(inspectable != null)
   {
    playerUI.ShowDescription(
        inspectable.GetDescription());
   }
    if(isInspecting)
    return;
    currentObject = obj;

    originalPosition = obj.transform.position;
    originalRotation = obj.transform.rotation;

    currentObject.transform.position =
    inspectPoint.position +
    inspectPoint.forward * inspectDistance;
    obj.transform.rotation = inspectPoint.rotation;

    isInspecting = true;
   }
}