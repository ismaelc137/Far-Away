using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private TextMeshProUGUI eventText;

    [Header("Player Control")]
    [SerializeField] private Player_Look playerLook;
    [SerializeField] private Player_Motor playerMotor;

    private Queue<PendingEvent> pendingEvents = new();
    private bool showingEvent;
    private Action currentOnConfirm;

    private struct PendingEvent
    {
        public string message;
        public Action onConfirm;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (eventPanel != null) eventPanel.SetActive(false);
    }

    private void Start()
    {
        if (playerLook == null) playerLook = FindAnyObjectByType<Player_Look>();
        if (playerMotor == null) playerMotor = FindAnyObjectByType<Player_Motor>();
    }

    public void QueueEvent(string message, Action onConfirm = null)
    {
        pendingEvents.Enqueue(new PendingEvent { message = message, onConfirm = onConfirm });
        if (!showingEvent) ShowNext();
    }

    public void QueueEventDelayed(string message, float delay, Action onConfirm = null)
    {
        StartCoroutine(DelayedQueue(message, delay, onConfirm));
    }

    private IEnumerator DelayedQueue(string message, float delay, Action onConfirm)
    {
        yield return new WaitForSeconds(delay);
        QueueEvent(message, onConfirm);
    }

    private void ShowNext()
    {
        if (pendingEvents.Count == 0)
        {
            showingEvent = false;
            ReleasePlayerControl();
            return;
        }

        if (!showingEvent)
            LockPlayerControl();

        showingEvent = true;
        PendingEvent next = pendingEvents.Dequeue();

        if (eventText != null) eventText.text = next.message;
        if (eventPanel != null) eventPanel.SetActive(true);

        currentOnConfirm = next.onConfirm;
    }

    public void CloseEvent()
    {
        if (eventPanel != null) eventPanel.SetActive(false);

        currentOnConfirm?.Invoke();
        currentOnConfirm = null;

        ShowNext();
    }

    private void LockPlayerControl()
    {
        CharacterController controller = playerMotor.GetComponent<CharacterController>();
        controller.enabled = false;
        if (playerLook != null) playerLook.lockLook = true;
        if (playerMotor != null) playerMotor.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ReleasePlayerControl()
    {
        CharacterController controller = playerMotor.GetComponent<CharacterController>();
        controller.enabled = true;
        if (playerLook != null) playerLook.lockLook = false;
        if (playerMotor != null) playerMotor.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}