using UnityEngine;
using UnityEngine.UI;
using static ResourcesTypes;

public class MetalPourMinigame : MonoBehaviour
{
    [Header("UI")]
    public RectTransform stream;
    public RectTransform mold;
    public Image fillBar;
    //public Sprite winSpirte;

    [Header("Stream Sprites")]
    public Sprite zelazoSprite;
    public Sprite zlotoSprite;
    public Sprite miedzSprite;

    [Header("Gameplay")]
    public float fillSpeed = 0.3f;
    public float streamAmplitude = 300f;
    public float streamSpeed = 400f;
    public float moldSpeed = 500f;

    private string currentMetal;
    private bool isPlaying = false;
    private float fillAmount = 0f;
    private float streamDirection = 1f;
    private MetalType usedMetal;

    private MinigameManager minigameManager;
    private void Start()
    {
        minigameManager = Dependencies.Instance.GetDependancy<MinigameManager>();
    }

    public void SetResource(string metal)
    {
        currentMetal = metal;

        Image img = stream.GetComponent<Image>();
        switch (metal.ToLower())
        {
            case "iron": img.sprite = zelazoSprite; usedMetal = MetalType.Iron; break;
            case "gold": img.sprite = zlotoSprite; usedMetal = MetalType.Gold; break;
            case "copper": img.sprite = miedzSprite; usedMetal = MetalType.Copper; break;
        }
    }

    public void InitializeMinigame()
    {
        fillAmount = 0f;
        fillBar.fillAmount = 0f;
        isPlaying = true;
        streamDirection = 1f;

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
        float input = Input.GetAxis("Horizontal");
        Vector2 pos = mold.anchoredPosition;
        pos.x += input * moldSpeed * Time.deltaTime;

        float halfWidth = mold.rect.width / 2f;
        float parentHalfWidth = (mold.parent as RectTransform).rect.width / 2f;
        pos.x = Mathf.Clamp(pos.x, -parentHalfWidth + halfWidth, parentHalfWidth - halfWidth);

        mold.anchoredPosition = pos;
    }

    void CheckFilling()
    {
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

        minigameManager.usedMetal = usedMetal;

        Invoke(nameof(ExitMinigame), 1.5f);
    }
    public void ResetState()
    {
        isPlaying = false;
        fillAmount = 0f;
        fillBar.fillAmount = 0f;

        if (stream != null)
            stream.GetComponent<Image>().sprite = null;

        if (stream != null)
            stream.anchoredPosition = Vector2.zero;

        if (mold != null)
            mold.anchoredPosition = Vector2.zero;
    }


    void ExitMinigame()
    {
      minigameManager.ExitMinigame();
    }
}
