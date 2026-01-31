using UnityEngine;
using UnityEngine.UI;

public class MetalPourMinigame : MonoBehaviour
{
    [Header("UI")]
    public RectTransform stream;
    public RectTransform mold; // kadŸ, któr¹ steruje gracz
    public Image fillBar;

    [Header("Stream Sprites")]
    public Sprite zelazoSprite;
    public Sprite zlotoSprite;
    public Sprite miedzSprite;

    [Header("Gameplay")]
    public float fillSpeed = 0.3f;       // ile wype³nia siê kadŸ na sekundê
    public float streamAmplitude = 300f; // szerokoœæ ruchu strumienia
    public float streamSpeed = 400f;     // teraz w pikselach/sekundê, zamiast 2f
    public float moldSpeed = 500f;       // prêdkoœæ poruszania kadzi¹
                                         // prêdkoœæ poruszania kadzi¹

    private string currentMetal;
    private bool isPlaying = false;
    private float fillAmount = 0f;

    private float streamDirection = 1f;

    // ================= INIT =================
    public void SetResource(string metal)
    {
        currentMetal = metal;

        Image img = stream.GetComponent<Image>();
        switch (metal.ToLower())
        {
            case "iron": img.sprite = zelazoSprite; break;
            case "gold": img.sprite = zlotoSprite; break;
            case "copper": img.sprite = miedzSprite; break;
            default: Debug.LogWarning("Nieznany metal: " + metal); break;
        }
    }

    public void InitializeMinigame()
    {
        fillAmount = 0f;
        fillBar.fillAmount = 0f;
        isPlaying = true;
        streamDirection = 1f;

        // Reset pozycji strumienia i kadzi
        if (stream != null)
            stream.anchoredPosition = new Vector2(0, stream.anchoredPosition.y);

        if (mold != null)
            mold.anchoredPosition = new Vector2(0, mold.anchoredPosition.y);
    }
    void Update()
    {
        if (!isPlaying) return;

        MoveStream();
        MoveMold();
        CheckFilling();
    }

    void MoveStream()
    {
        if (stream == null) return;

        Vector2 pos = stream.anchoredPosition;
        pos.x += streamDirection * streamSpeed * Time.deltaTime;

        if (pos.x > streamAmplitude / 2f)
        {
            pos.x = streamAmplitude / 2f;
            streamDirection *= -1f;
        }
        else if (pos.x < -streamAmplitude / 2f)
        {
            pos.x = -streamAmplitude / 2f;
            streamDirection *= -1f;
        }

        stream.anchoredPosition = pos;
    }

    void MoveMold()
    {
        float input = Input.GetAxis("Horizontal"); // strza³ki A/D lub lewo/prawo
        Vector2 pos = mold.anchoredPosition;
        pos.x += input * moldSpeed * Time.deltaTime;

        // ograniczenie kadzi w granicach ekranu/rodzica
        float halfWidth = mold.rect.width / 2f;
        float parentHalfWidth = (mold.parent as RectTransform).rect.width / 2f;
        pos.x = Mathf.Clamp(pos.x, -parentHalfWidth + halfWidth, parentHalfWidth - halfWidth);

        mold.anchoredPosition = pos;
    }

    void CheckFilling()
    {
        // sprawdzamy czy kadŸ "³apie" strumieñ
        float streamX = stream.anchoredPosition.x;
        float moldX = mold.anchoredPosition.x;

        float moldHalfWidth = mold.rect.width / 2f;

        if (streamX >= moldX - moldHalfWidth && streamX <= moldX + moldHalfWidth)
        {
            fillAmount += fillSpeed * Time.deltaTime;
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
