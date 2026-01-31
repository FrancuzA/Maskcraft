using UnityEngine;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    [Header("References")]
    public GameObject minigameCanvas;
    public PlayerMovement playerMovement;
    public CarvingMinigame carvingMinigame;

    private bool isMinigameActive = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (minigameCanvas != null)
            minigameCanvas.SetActive(false);
    }

    public void EnterMinigame()
    {
        if (isMinigameActive) return;
        isMinigameActive = true;

        // Disable player movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Show cursor for UI interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Enable minigame UI
        if (minigameCanvas != null)
            minigameCanvas.SetActive(true);

        // Initialize after 1 frame to ensure Canvas is fully active
        if (carvingMinigame != null)
        {
            carvingMinigame.gameObject.SetActive(true);
            StartCoroutine(DelayedInitialize());
        }
    }

    private IEnumerator DelayedInitialize()
    {
        yield return null;
        carvingMinigame.InitializeMinigame();
    }

    public void ExitMinigame()
    {
        if (!isMinigameActive) return;
        isMinigameActive = false;

        // Restore player movement
        if (playerMovement != null)
            playerMovement.enabled = true;

        // Hide cursor for FPS gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Hide minigame UI
        if (minigameCanvas != null)
            minigameCanvas.SetActive(false);
    }

    public bool IsMinigameActive() => isMinigameActive;
}