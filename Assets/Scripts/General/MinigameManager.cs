using UnityEngine;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    [Header("References")]
    public GameObject minigameCanvas;
    public PlayerMovement playerMovement;

    [Header("Minigames")]
    public CarvingMinigame carvingMinigame;
    public MetalPourMinigame metalPourMinigame;

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

        isMinigameActive = false; // pewność, że na start nie blokuje gracza
    }


    // Odpalona przez stół
    public void EnterMinigame(string minigameName)
    {
        if (isMinigameActive) return;
        isMinigameActive = true;

        // Disable player movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Show cursor for UI interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Enable minigame canvas
        if (minigameCanvas != null)
            minigameCanvas.SetActive(true);

        // Włącz odpowiednią minigierkę
        if (minigameName == "Carving" && carvingMinigame != null)
        {
            carvingMinigame.gameObject.SetActive(true);
            StartCoroutine(DelayedInitialize(carvingMinigame));
        }
        else if (minigameName == "MetalPour" && metalPourMinigame != null)
        {
            metalPourMinigame.gameObject.SetActive(true);
            StartCoroutine(DelayedInitialize(metalPourMinigame));
        }
    }

    private IEnumerator DelayedInitialize(MonoBehaviour minigame)
    {
        yield return null;

        if (minigame is CarvingMinigame carve)
            carve.InitializeMinigame();
        else if (minigame is MetalPourMinigame pour)
            pour.InitializeMinigame();
    }

    public void ExitMinigame()
    {
        if (!isMinigameActive) return;
        isMinigameActive = false;

        // Restore player movement
        if (playerMovement != null)
            playerMovement.enabled = true;

        // Hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Hide minigame canvas
        if (minigameCanvas != null)
            minigameCanvas.SetActive(false);

        // Dezaktywuj wszystkie minigierki
        if (carvingMinigame != null)
            carvingMinigame.gameObject.SetActive(false);
        if (metalPourMinigame != null)
            metalPourMinigame.gameObject.SetActive(false);
    }

    public bool IsMinigameActive() => isMinigameActive;
}
