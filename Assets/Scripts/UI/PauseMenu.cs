using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private const string MouseSensitivityKey = "MouseSensitivity";
    private const float DefaultMouseSensitivity = 2f;

    public GameObject settingsPanel;
    public Player_Look playerLook;
    public Slider sensitivitySlider;

    private bool isOpen = false;

    private void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (playerLook == null)
            playerLook = FindFirstObjectByType<Player_Look>();

        float savedSensitivity = PlayerPrefs.GetFloat(MouseSensitivityKey, DefaultMouseSensitivity);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 1f;
            sensitivitySlider.maxValue = 5f;
            sensitivitySlider.SetValueWithoutNotify(savedSensitivity);
        }

        ApplySensitivity(savedSensitivity);
    }

    private void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.tabKey.wasPressedThisFrame)
            return;

        if (isOpen)
        {
            ToggleMenu();
            return;
        }

        // Do not open pause over an event, terminal, ladder, or sleep transition.
        if (playerLook != null && !playerLook.lockLook &&
            (EventManager.Instance == null || !EventManager.Instance.IsShowingEvent))
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        isOpen = !isOpen;
        if (settingsPanel != null)
            settingsPanel.SetActive(isOpen);

        if (playerLook != null)
            playerLook.lockLook = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;

        Time.timeScale = isOpen ? 0f : 1f;
    }

    public void OnSensitivityChanged(float value)
    {
        ApplySensitivity(value);
        PlayerPrefs.SetFloat(MouseSensitivityKey, value);
        PlayerPrefs.Save();
    }

    private void ApplySensitivity(float value)
    {
        if (playerLook != null)
            playerLook.SetSensitivity(value);
    }

    private void OnDisable()
    {
        if (!isOpen)
            return;

        isOpen = false;
        Time.timeScale = 1f;
        if (playerLook != null)
            playerLook.lockLook = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
