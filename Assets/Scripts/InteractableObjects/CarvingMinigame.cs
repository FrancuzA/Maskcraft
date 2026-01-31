using UnityEngine;
using UnityEngine.UI;

public class CarvingMinigame : MonoBehaviour
{
    [Header("Setup")]
    public RawImage carvingSurface;   // Brown wood background
    public RawImage guideOverlay;      // White outline on transparent BG (Read/Write Enabled!)
    public RawImage carvedLines;       // Top layer for white carved lines

    [Header("Gameplay")]
    public float brushSize = 12f;
    public float coverageThreshold = 0.8f; // 80% outline coverage to win
    public bool showDebugInfo = true;

    [Header("Feedback")]
    public GameObject successPanel;

    private Texture2D woodTexture;
    private Texture2D linesTexture;
    private Texture2D guideTexture;
    private Color32[] guidePixels;
    private int totalOutlinePixels = 0;
    private int coveredOutlinePixels = 0;
    private bool isCarving = false;
    private bool minigameActive = false;

    void Start()
    {
        SetupTextures();
        CountOutlinePixels();
        StartMinigame();
    }

    void SetupTextures()
    {
        // Validate
        if (!carvingSurface || !guideOverlay || !carvedLines)
        {
            Debug.LogError("❌ Missing RawImage references in Inspector!");
            enabled = false;
            return;
        }

        // Wood background (static brown)
        woodTexture = new Texture2D(512, 512, TextureFormat.ARGB32, false);
        FillTexture(woodTexture, new Color32(102, 66, 33, 255)); // Rich wood brown
        woodTexture.Apply();
        carvingSurface.texture = woodTexture;

        // Carved lines layer (starts transparent)
        linesTexture = new Texture2D(512, 512, TextureFormat.ARGB32, false);
        linesTexture.filterMode = FilterMode.Point;
        ClearTexture(linesTexture);
        linesTexture.Apply();
        carvedLines.texture = linesTexture;

        // Guide texture
        guideTexture = guideOverlay.mainTexture as Texture2D;
        if (!guideTexture)
        {
            Debug.LogError("❌ Guide texture missing or not readable! Enable 'Read/Write Enabled' in Import Settings.");
            enabled = false;
            return;
        }
        guidePixels = guideTexture.GetPixels32();
    }

    void CountOutlinePixels()
    {
        totalOutlinePixels = 0;
        foreach (var col in guidePixels)
            if (col.r > 0.5f && col.a > 0.1f) totalOutlinePixels++;

        if (totalOutlinePixels == 0)
            Debug.LogWarning("⚠️ Guide texture has NO visible outline pixels! Check transparency.");
    }

    void StartMinigame()
    {
        // 🔑 CRITICAL: Force cursor VISIBLE before anything else
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        minigameActive = true;
        coveredOutlinePixels = 0;
        guideOverlay.enabled = true;
        ClearTexture(linesTexture);
        linesTexture.Apply();

        if (showDebugInfo)
            Debug.Log($"🎯 Trace {totalOutlinePixels} outline pixels to complete mask");
    }

    void Update()
    {
        if (!minigameActive) return;

        // 🔑 REINFORCE cursor visibility every frame (defeats FPS controllers)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (Input.GetMouseButtonDown(0) && IsPointerOverCarvingArea())
        {
            isCarving = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCarving = false;
        }

        if (isCarving && Input.GetMouseButton(0))
        {
            Vector2 localPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                carvingSurface.rectTransform, Input.mousePosition, null, out localPos))
            {
                int texX = Mathf.FloorToInt((localPos.x / carvingSurface.rectTransform.rect.width + 0.5f) * linesTexture.width);
                int texY = Mathf.FloorToInt((localPos.y / carvingSurface.rectTransform.rect.height + 0.5f) * linesTexture.height);
                DrawCarvedLine(texX, texY);
            }
        }

        // Auto-complete check
        if (totalOutlinePixels > 0)
        {
            float coverage = (float)coveredOutlinePixels / totalOutlinePixels;
            if (coverage >= coverageThreshold)
            {
                CompleteMinigame(coverage);
            }
            else if (showDebugInfo && Input.GetKeyDown(KeyCode.C)) // Debug key
            {
                Debug.Log($"📊 Coverage: {(coverage * 100):F0}% ({coveredOutlinePixels}/{totalOutlinePixels})");
            }
        }
    }

    bool IsPointerOverCarvingArea()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            carvingSurface.rectTransform, Input.mousePosition, null);
    }

    void DrawCarvedLine(int centerX, int centerY)
    {
        int radius = Mathf.FloorToInt(brushSize / 2);
        Color32 carvedColor = new Color32(240, 240, 240, 255); // Bright white

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                int px = centerX + x;
                int py = centerY + y;
                if (px < 0 || px >= linesTexture.width || py < 0 || py >= linesTexture.height) continue;
                if (Mathf.Sqrt(x * x + y * y) > radius) continue;

                // Draw white line
                linesTexture.SetPixel(px, py, carvedColor);

                // Check coverage (only count once per outline pixel)
                int idx = py * guideTexture.width + px;
                if (idx >= 0 && idx < guidePixels.Length)
                {
                    Color32 guideCol = guidePixels[idx];
                    if (guideCol.r > 0.5f && guideCol.a > 0.1f)
                    {
                        coveredOutlinePixels++;
                        // Mark as covered to avoid double-counting
                        guidePixels[idx] = new Color32(0, 0, 0, 0);
                    }
                }
            }
        }
        linesTexture.Apply(false);
    }

    void CompleteMinigame(float coverage)
    {
        minigameActive = false;
        guideOverlay.enabled = false;

        Debug.Log($"✅ MASK COMPLETE! Coverage: {(coverage * 100):F0}%");
        if (successPanel) successPanel.SetActive(true);

        // Optional: Auto-close after 3 seconds
        Invoke("HideCursorAfterDelay", 3f);
    }

    void HideCursorAfterDelay()
    {
        // Only hide if returning to FPS mode
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void ClearTexture(Texture2D tex)
    {
        Color32[] clear = new Color32[tex.width * tex.height];
        for (int i = 0; i < clear.Length; i++) clear[i] = new Color32(0, 0, 0, 0);
        tex.SetPixels32(clear);
    }

    void FillTexture(Texture2D tex, Color32 color)
    {
        Color32[] fill = new Color32[tex.width * tex.height];
        for (int i = 0; i < fill.Length; i++) fill[i] = color;
        tex.SetPixels32(fill);
    }

    // Optional: Reset via UI button
    public void ResetMinigame()
    {
        StartMinigame();
    }
}