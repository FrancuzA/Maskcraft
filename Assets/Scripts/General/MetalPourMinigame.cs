using UnityEngine;
using UnityEngine.UI;

public class MetalPourMinigame : MonoBehaviour
{
    [Header("UI")]
    public RectTransform stream;
    public RectTransform mold;
    public Image fillBar;

    [Header("Stream Sprites")]
    public Sprite zelazoSprite;
    public Sprite zlotoSprite;
    public Sprite miedzSprite;

    [Header("Stream movement")]
    public float amplitude = 300f;
    public float speed = 2f;

    [Header("Gameplay")]
    public float tolerance = 50f;
    public float fillSpeed = 0.3f;

    [Header("Metal")]
    public float totalMetal = 200f;
    public float pourSpeed = 10f;

    private float fillAmount = 0f;
    private float metalLeft;
    private bool isPlaying = false;

    private string currentMetal;

    // ================= INIT =================
    public void SetResource(string metal)
    {
        currentMetal = metal;

        Image img = stream.GetComponent<Image>();
        switch (metal)
        {
            case "iron": img.sprite = zelazoSprite; break;
            case "gold": img.sprite = zlotoSprite; break;
            case "copper": img.sprite = miedzSprite; break;
        }
    }

    public void InitializeMinigame()
    {
        metalLeft = totalMetal;
        fillAmount = 0f;
        isPlaying = true;
        fillBar.fillAmount = 0f;
    }

    void Update()
    {
        if (!isPlaying) return;

        // Animacja strumienia
        float x = Mathf.Sin(Time.time * speed) * amplitude;
        stream.anchoredPosition = new Vector2(x, stream.anchoredPosition.y);

        // Pour
        if (Input.GetMouseButton(0) && metalLeft > 0f)
        {
            fillAmount += fillSpeed * Time.deltaTime;
            metalLeft -= pourSpeed * Time.deltaTime;

            fillAmount = Mathf.Clamp01(fillAmount);
            fillBar.fillAmount = fillAmount;
        }

        if (fillAmount >= 1f)
            WinMinigame();
    }

    void WinMinigame()
    {
        isPlaying = false;
        fillBar.fillAmount = 1f;
        Invoke(nameof(ExitMinigame), 1.5f);
    }

    void ExitMinigame()
    {
        MinigameManager.Instance.ExitMinigame();
    }
}
