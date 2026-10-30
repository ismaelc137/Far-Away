using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private const string MouseSensitivityKey = "MouseSensitivity";
    private const float DefaultMouseSensitivity = 2f;

    [SerializeField] private Slider sensitivitySlider;

    private void Awake()
    {
        if (sensitivitySlider == null)
            sensitivitySlider = GetComponent<Slider>();

        if (sensitivitySlider == null)
        {
            Debug.LogError("SettingsMenu needs a sensitivity slider.", this);
            return;
        }

        sensitivitySlider.minValue = 1f;
        sensitivitySlider.maxValue = 5f;
        sensitivitySlider.onValueChanged.AddListener(ApplySensitivity);

        float savedSensitivity = PlayerPrefs.GetFloat(MouseSensitivityKey, DefaultMouseSensitivity);
        sensitivitySlider.SetValueWithoutNotify(savedSensitivity);
        ApplySensitivity(savedSensitivity);
    }

    private void OnDestroy()
    {
        if (sensitivitySlider != null)
            sensitivitySlider.onValueChanged.RemoveListener(ApplySensitivity);
    }

    private static void ApplySensitivity(float value)
    {
        PlayerPrefs.SetFloat(MouseSensitivityKey, value);
        PlayerPrefs.Save();

        Player_Look playerLook = FindFirstObjectByType<Player_Look>();
        if (playerLook != null)
            playerLook.SetSensitivity(value);
    }
}
