using UnityEngine;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    [Header("Core")]
    public GameObject minigameCanvas;
    public PlayerMovement playerMovement;

    [Header("Minigames")]
    public CarvingMinigame carvingMinigame;
    public MetalPourMinigame metalPourMinigame;
    public MaskPaintingMinigame maskPaintingMinigame;

    private bool isMinigameActive = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ================= ENTER =================

    public void EnterMinigame(string name)
    {
        if (isMinigameActive) return;
        isMinigameActive = true;

        if (playerMovement != null)
            playerMovement.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (minigameCanvas != null)
            minigameCanvas.SetActive(true);

        DisableAllMinigames();

        switch (name)
        {
            case "Carving":
                carvingMinigame.gameObject.SetActive(true);
                carvingMinigame.InitializeMinigame();
                break;

            case "MetalPour":
                metalPourMinigame.gameObject.SetActive(true);
                metalPourMinigame.InitializeMinigame();
                break;

            case "MaskPainting":
                maskPaintingMinigame.gameObject.SetActive(true);
                maskPaintingMinigame.InitializeMinigame();
                break;
        }
    }

    // ================= EXIT =================

    public void ExitMinigame()
    {
        if (!isMinigameActive) return;
        isMinigameActive = false;

        if (playerMovement != null)
            playerMovement.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (minigameCanvas != null)
            minigameCanvas.SetActive(false);

        DisableAllMinigames();
    }

    void DisableAllMinigames()
    {
        carvingMinigame.gameObject.SetActive(false);
        metalPourMinigame.gameObject.SetActive(false);
        maskPaintingMinigame.gameObject.SetActive(false);
    }

    // painting hook
    // specjalny hook dla painting
    public void OnPaintingFinished(PaintingTable table)
    {
        // uruchomienie Coroutine z aktywnego obiektu (MinigameManager)
        StartCoroutine(ReenableTableNextFrame(table));
    }

    private IEnumerator ReenableTableNextFrame(PaintingTable table)
    {
        yield return null; // 1 frame opóźnienia
        table.gameObject.SetActive(true);
    }

    public bool IsMinigameActive()
    {
        return isMinigameActive;
    }
}

