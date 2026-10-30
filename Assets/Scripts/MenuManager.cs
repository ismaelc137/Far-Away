using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
            return;

        Canvas menuCanvas = FindFirstObjectByType<Canvas>();
        if (menuCanvas == null)
            return;

        GameObject noticeObject = new GameObject("Prototype Notice", typeof(RectTransform), typeof(TextMeshProUGUI));
        noticeObject.transform.SetParent(menuCanvas.transform, false);

        RectTransform noticeRect = noticeObject.GetComponent<RectTransform>();
        noticeRect.anchorMin = new Vector2(0.08f, 0f);
        noticeRect.anchorMax = new Vector2(0.92f, 0f);
        noticeRect.pivot = new Vector2(0.5f, 0f);
        noticeRect.anchoredPosition = new Vector2(0f, 24f);
        noticeRect.sizeDelta = new Vector2(0f, 54f);

        TextMeshProUGUI noticeText = noticeObject.GetComponent<TextMeshProUGUI>();
        noticeText.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF")
            ?? TMP_Settings.defaultFontAsset;
        noticeText.text = "<b>EARLY PROTOTYPE</b>\nThis build is far from the final Far Away experience.";
        noticeText.alignment = TextAlignmentOptions.Center;
        noticeText.color = new Color(0.92f, 0.92f, 0.88f, 1f);
        noticeText.fontSize = 18f;
        noticeText.enableAutoSizing = true;
        noticeText.fontSizeMin = 13f;
        noticeText.fontSizeMax = 18f;
        noticeText.raycastTarget = false;

        Shadow textShadow = noticeObject.AddComponent<Shadow>();
        textShadow.effectColor = new Color(0f, 0f, 0f, 0.85f);
        textShadow.effectDistance = new Vector2(1f, -1f);
    }

    public void LoadScene(string n)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(n);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
