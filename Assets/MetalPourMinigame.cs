using UnityEngine;
using UnityEngine.UI;

public class MetalPourMinigame : MonoBehaviour
{
    [Header("UI")]
    public RectTransform stream;
    public RectTransform mold;
    public Image fillBar;

    [Header("Stream movement")]
    public float amplitude = 300f;
    public float speed = 2f;

    [Header("Gameplay")]
    public float tolerance = 50f;
    public float fillSpeed = 0.3f;

    [Header("Metal")]
    public float totalMetal = 200f;   // ile metalu
    public float pourSpeed = 10f;     // jak szybko ubywa

    private float fillAmount = 0f;
    private float metalLeft;
    private bool isPlaying = true;

    void Start()
    {
        metalLeft = totalMetal;
        fillBar.fillAmount = 0f;
    }

    void Update()
    {
        if (!isPlaying) return;

        MoveStream();
        MoveMold();
        PourLogic();
    }

    // ---------- STRUMIEÑ ----------

    void MoveStream()
    {
        float x = Mathf.Sin(Time.time * speed) * amplitude;
        stream.anchoredPosition = new Vector2(x, stream.anchoredPosition.y);
    }

    // ---------- FORMA (TYLKO X) ----------

    void MoveMold()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            stream.parent as RectTransform,
            Input.mousePosition,
            null,
            out mousePos
        );

        mold.anchoredPosition = new Vector2(mousePos.x, mold.anchoredPosition.y);
    }

    // ---------- LOGIKA LANIA ----------

    void PourLogic()
    {
        if (metalLeft > 0)
        {
            metalLeft -= pourSpeed * Time.deltaTime;

            float distance = Mathf.Abs(stream.anchoredPosition.x - mold.anchoredPosition.x);

            if (distance < tolerance)
            {
                fillAmount += fillSpeed * Time.deltaTime;
                fillAmount = Mathf.Clamp01(fillAmount);
                fillBar.fillAmount = fillAmount;
            }
        }
        else
        {
            EndMinigame();
        }
    }

    // ---------- KONIEC + OCENA ----------

    void EndMinigame()
    {
        isPlaying = false;

        Debug.Log("FINAL FILL: " + fillAmount);

        if (fillAmount > 0.9f)
            Debug.Log("PERFECT MASK");
        else if (fillAmount > 0.6f)
            Debug.Log("GOOD MASK");
        else if (fillAmount > 0.3f)
            Debug.Log("BAD MASK");
        else
            Debug.Log("YOU FUCKED IT");
    }
    public void InitializeMinigame()
    {
        fillAmount = 0f;
        metalLeft = totalMetal;
        isPlaying = true;
        fillBar.fillAmount = 0f;
    }

}
