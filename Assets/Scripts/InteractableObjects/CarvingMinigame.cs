using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ResourcesTypes;

public class CarvingMinigame : MonoBehaviour
{
    [Header("Setup")]
    public RawImage drawingCanvas;
    public Texture2D maskReferenceAkacja;
    public Texture2D maskReferenceWierzba;
    public Texture2D maskReferencePalma;
    public Texture2D guideMaskReferenceAkacja;
    public Texture2D guideMaskReferenceWierzba;
    public Texture2D guideMaskReferencePalma;
    public RawImage maskGuide;
    public TMP_Text similarityText;

    [Header("Gameplay")]
    [Range(10, 50)] public float brushSize = 25f;
    [Range(0.3f, 0.8f)] public float winThreshold = 0.5f;

    private Texture2D drawTexture;
    private Color32[] maskPixels;
    private int totalMaskPixels = 0;
    private bool hasWon = false;
    private bool isInitialized = false;
    private Texture2D maskReference;
    private WoodType currentWood;

    public void SetResource(string resource)
    {
        switch (resource)
        {
            case "acacia":
                maskReference = maskReferenceAkacja;
                maskGuide.texture = guideMaskReferenceAkacja;
                currentWood = WoodType.Acacia;
                break;
            case "willow":
                maskReference = maskReferenceWierzba;
                maskGuide.texture = guideMaskReferenceWierzba;
                currentWood = WoodType.Willow;
                break;
            case "palm":
                maskReference = maskReferencePalma;
                maskGuide.texture = guideMaskReferencePalma;
                currentWood = WoodType.Palm;
                break;
        }
    }

    public void InitializeMinigame()
    {
        drawTexture = new Texture2D(256, 256, TextureFormat.ARGB32, false);
        drawingCanvas.texture = drawTexture;

        maskPixels = maskReference.GetPixels32();
        totalMaskPixels = 0;
        foreach (var col in maskPixels)
            if (col.r > 0.5f) totalMaskPixels++;

        ClearTexture(drawTexture, Color.black);

        hasWon = false;
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized || hasWon) return;

        if (Input.GetMouseButton(0))
        {
            Vector2 localPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                drawingCanvas.rectTransform, Input.mousePosition, null, out localPos))
            {
                int x = Mathf.FloorToInt((localPos.x / drawingCanvas.rectTransform.rect.width + 0.5f) * 256);
                int y = Mathf.FloorToInt((localPos.y / drawingCanvas.rectTransform.rect.height + 0.5f) * 256);
                DrawBrush(x, y);
            }
        }

        if (Time.frameCount % 18 == 0)
        {
            float similarity = CalculateSimilarity();
            similarityText.text = $"Coverage: {(similarity * 100):F0}%";

            if (similarity >= winThreshold)
                WinMinigame();
        }
    }

    void DrawBrush(int centerX, int centerY)
    {
        int radius = Mathf.FloorToInt(brushSize / 2);
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                int px = centerX + x;
                int py = centerY + y;

                if (px < 0 || px >= 256 || py < 0 || py >= 256) continue;
                if (Mathf.Sqrt(x * x + y * y) > radius) continue;

                drawTexture.SetPixel(px, py, Color.white);
            }
        }
        drawTexture.Apply(false);
    }

    float CalculateSimilarity()
    {
        Color32[] drawn = drawTexture.GetPixels32();
        int drawnOnMask = 0;

        for (int i = 0; i < drawn.Length; i++)
        {
            if (maskPixels[i].r > 0.5f && drawn[i].r > 0.5f)
                drawnOnMask++;
        }

        return (float)drawnOnMask / totalMaskPixels;
    }

    void WinMinigame()
    {
        hasWon = true;
        similarityText.text = "✅ COMPLETE";

        // zapisujemy użyty wood do MinigameManager
        MinigameManager.Instance.usedWood = currentWood;

        Invoke(nameof(ExitMinigame), 1.5f);
    }



    void ExitMinigame()
    {
        MinigameManager.Instance.ExitMinigame();
    }

    void ClearTexture(Texture2D tex, Color color)
    {
        Color32[] fill = new Color32[tex.width * tex.height];
        Color32 col = color;
        for (int i = 0; i < fill.Length; i++)
            fill[i] = col;

        tex.SetPixels32(fill);
        tex.Apply();
    }
}
