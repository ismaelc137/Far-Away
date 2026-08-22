using UnityEngine;
using System.Collections;
using System.Linq.Expressions;

public class LadderController : MonoBehaviour
{
    public bool IsOnLadder { get; private set; }
    private bool lastClimbingState;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Player_Look playerLook;
    [SerializeField] private Player_Motor motor;
    [SerializeField] private CharacterController controller;

    [SerializeField] private float ladderOffset = 0.3f;
    [SerializeField] private float exitDistance = 0.2f;
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private Hands hands;
    private LadderInteractable currentLadder;

    public void EnterLadder(LadderInteractable ladder)
    {
        if (IsOnLadder)
            return;

        currentLadder = ladder;
        StartCoroutine(EnterLadderRoutine(ladder));
    }

    public void ExitLadder()
    {
        
        currentLadder.ladderBlocker.SetActive(true);
        IsOnLadder = false;
        motor.IsClimbing = false;
        playerLook.lockLook = false;
        currentLadder = null;
    }

    private IEnumerator EnterLadderRoutine(LadderInteractable ladder)
    {
        currentLadder.ladderBlocker.SetActive(false);
        IsOnLadder = true;
        motor.IsClimbing = true;
        float distToTop = Vector3.Distance(
            playerTransform.position,
            ladder.topPoint.position);

        float distToBottom = Vector3.Distance(
            playerTransform.position,
            ladder.bottomPoint.position);

        Vector3 targetPosition;

        if (distToTop < distToBottom)
        {
            // Entrando desde arriba
            targetPosition = ladder.topPoint.position + Vector3.down * ladderOffset;
        }
        else
        {
            // Entrando desde abajo
            targetPosition = ladder.bottomPoint.position + Vector3.up * ladderOffset;
        }

        controller.enabled = false;
        playerTransform.position = targetPosition;
        controller.enabled = true;

        Quaternion startRotation = playerTransform.rotation;

        Vector3 forward = ladder.transform.forward;
        forward.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(forward);

        float timer = 0f;

        while (timer < rotationDuration)
        {
            timer += Time.deltaTime;

            playerTransform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                timer / rotationDuration);

            yield return null;
        }

        playerTransform.rotation = targetRotation;

        playerLook.ResetLook();
        playerLook.lockLook = true;
    }

    private void Update()
    {
        if (!IsOnLadder || currentLadder == null)
            return;
        if (motor.IsMovingOnLadder != lastClimbingState)
       {
        hands.SetClimbing(motor.IsMovingOnLadder);
        lastClimbingState = motor.IsMovingOnLadder;
       }

        float topDistance = Vector3.Distance(
            playerTransform.position,
            currentLadder.topPoint.position);

        float bottomDistance = Vector3.Distance(
            playerTransform.position,
            currentLadder.bottomPoint.position);

        if (topDistance < exitDistance)
        {
            controller.enabled = false;
            playerTransform.position = currentLadder.topPoint.position;
            controller.enabled = true;

            ExitLadder();
            playerLook.lockLook = false;
        }

        if (bottomDistance < exitDistance)
        {
            controller.enabled = false;
            playerTransform.position = currentLadder.bottomPoint.position;
            controller.enabled = true;
            ExitLadder();
        }
    }
}