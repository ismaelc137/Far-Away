using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayeUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [Header("Energy")]
    [SerializeField]
    private Image energyFill;

    [SerializeField]
    private TextMeshProUGUI energyText;

    [SerializeField]
    private string noEnergyMessage = "No energy. Rest in bed.";
    private float temporaryMessageEndTime;

    public void ShowTemporaryMessage(string message, float duration = 2f)
    {
     if (promptText == null)
         return;

     temporaryMessageEndTime = Time.time + duration;
     promptText.text = message;
    }
    private void Update()
    {
        UpdateEnergy();
    }

    // Update is called once per frame
    public void UpdateText(string promptMessage)
    {
        if (promptText == null)
            return;
        if (Time.time < temporaryMessageEndTime)
        return;
        promptText.text = promptMessage;
    }

    public void ShowNoEnergyMessage()
    {
        UpdateText(noEnergyMessage);
    }

    private void UpdateEnergy()
    {
        if (GameManager.Instance == null)
            return;

        float energyPercent = GameManager.Instance.EnergyPercent;

        if (energyFill != null)
        {
            energyFill.fillAmount = energyPercent;
        }

        if (energyText != null)
        {
            energyText.text =
                $"{Mathf.CeilToInt(GameManager.Instance.energy)} / {Mathf.CeilToInt(GameManager.Instance.maxEnergy)}";
        }
    }

    public void ShowDescription(string description)
    {
        if (descriptionText == null)
            return;

        descriptionText.gameObject.SetActive(true);
        descriptionText.text = description;
    }

    public void HideDescription()
    {
        if (descriptionText == null)
            return;

        descriptionText.gameObject.SetActive(false);
    }
}
