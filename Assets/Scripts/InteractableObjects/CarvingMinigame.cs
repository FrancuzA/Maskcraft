using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarvingMinigame : MonoBehaviour
{
    [Header("Setup")]
    public RawImage drawingCanvas;
    public Texture2D maskReference;
    public TMP_Text similarityText;

    [Header("Gameplay")]
    [Range(10, 50)] public float brushSize = 25f;
    [Range(0.3f, 0.8f)] public float winThreshold = 0.5f;

    private Texture2D drawTexture;
    private Color32[] maskPixels;
    private bool hasWon = false;
    private int totalMaskPixels = 0;
    private bool isInitialized = false;

    // Initialize texture early to avoid null references on first interaction
    void Awake()
    {
        drawTexture = new Texture2D(256, 256, TextureFormat.ARGB32, false);
        ClearTexture(drawTexture, Color.black);
        drawTexture.Apply();
    }

    void Start()
    {
        // Assign texture to RawImage if not already set
        if (drawingCanvas != null && drawingCanvas.texture == null)
        {
            drawingCanvas.texture = drawTexture;
        }
    }

    // Called by MinigameManager when player starts minigame
    public void InitializeMinigame()
    {
        // Validate mask texture setup
        if (maskReference == null)
        {
            Debug.LogError("maskReference not assigned in CarvingMinigame!");
            return;
        }

        // Load mask pixels and count white areas (the shape to trace)
        maskPixels = maskReference.GetPixels32();
        totalMaskPixels = 0;
        foreach (var col in maskPixels)
        {
            if (col.r > 0.5f) totalMaskPixels++;
        }

        if (totalMaskPixels == 0)
        {
            Debug.LogError("maskReference has no white pixels! Must be white shape on black background.");
            return;
        }

        // Reset drawing state
        ClearTexture(drawTexture, Color.black);
        drawTexture.Apply();
        hasWon = false;
        isInitialized = true;

        // Show cursor for drawing
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (!isInitialized || hasWon) return;

        // Draw white brush strokes where player clicks
        if (Input.GetMouseButton(0) && drawingCanvas != null && IsPointerOver(drawingCanvas.rectTransform))
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

        // Check completion every 0.3 seconds
        if (Time.frameCount % 18 == 0 && totalMaskPixels > 0)
        {
            float similarity = CalculateSimilarity();

            if (similarityText != null)
                similarityText.text = $"Coverage: {(similarity * 100):F0}%";

            if (similarity >= winThreshold && !hasWon)
            {
                WinMinigame(similarity);
            }
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

    // Calculate % of mask shape covered by player's drawing
    float CalculateSimilarity()
    {
        Color32[] drawn = drawTexture.GetPixels32();
        int drawnOnMask = 0;

        for (int i = 0; i < drawn.Length; i++)
        {
            if (maskPixels[i].r > 0.5f && drawn[i].r > 0.5f)
                drawnOnMask++;
        }

        return totalMaskPixels > 0 ? (float)drawnOnMask / totalMaskPixels : 0f;
    }

    void WinMinigame(float similarity)
    {
        hasWon = true;
        if (similarityText != null)
            similarityText.text = $"✅ COMPLETE {(similarity * 100):F0}%";

        Invoke("ExitMinigame", 2f);
    }

    void ExitMinigame()
    {
        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.ExitMinigame();
        }
        else
        {
            // Fallback cursor state if manager missing
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void ClearTexture(Texture2D tex, Color color)
    {
        if (tex == null) return;
        Color32[] fill = new Color32[tex.width * tex.height];
        Color32 col = (Color32)color;
        for (int i = 0; i < fill.Length; i++) fill[i] = col;
        tex.SetPixels32(fill);
        tex.Apply();
    }

    bool IsPointerOver(RectTransform rt)
    {
        if (rt == null) return false;
        return RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition, null);
    }
}