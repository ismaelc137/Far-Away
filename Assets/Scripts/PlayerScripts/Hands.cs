using UnityEngine;

public class Hands : MonoBehaviour
{
    public Vector3 baseRotation;
    public Transform cameraTransform;
    [SerializeField] private Player_Motor motor;
    private Vector3 startPos;
    private float bobTimer;
    [SerializeField] private Animator rightHand;
    [SerializeField] private Animator leftHand;

    public void Start()
    {
     startPos = transform.localPosition;
    }
    public void Update()
    {
    if (motor.IsMoving)
    {
    bobTimer += Time.deltaTime * 8f;

    float x = Mathf.Cos(bobTimer * 0.5f) * 0.01f;
    float y = Mathf.Sin(bobTimer) * 0.02f;

    transform.localPosition = startPos + new Vector3(x, y, 0);
    SetClimbing(motor.IsMovingOnLadder);
    }
    else
    {
    bobTimer = 0;

    transform.localPosition = Vector3.Lerp(
        transform.localPosition,
        startPos,
        Time.deltaTime * 10f
    );
    }
    }
    public Hands(Transform cameraTransform)
    {
        this.cameraTransform = cameraTransform;
    }

    [Range(0f,1f)]
    public float pitchInfluence = 0f;
    public void ShowHands()
    {
    gameObject.SetActive(true);
    }

    public void HideHands()
    {
    gameObject.SetActive(false);
    }
    public void SetClimbing(bool climbing)
    {
        rightHand.SetBool("IsClimbing", climbing);
        leftHand.SetBool("IsClimbing", climbing);
    }

    void LateUpdate()
    {
    float pitch = cameraTransform.localEulerAngles.x;

    if (pitch > 180)
        pitch -= 360;

        transform.localRotation =
        Quaternion.Euler(
            baseRotation.x + pitch * pitchInfluence,
            baseRotation.y,
            baseRotation.z
        );
    }
}
