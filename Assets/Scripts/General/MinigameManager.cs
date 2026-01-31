using UnityEngine;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    public GameObject minigameCanvas;
    public PlayerMovement playerMovement;

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

    public void EnterMinigame(string name)
    {
        if (isMinigameActive) return;
        isMinigameActive = true;

        playerMovement.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        minigameCanvas.SetActive(true);
        maskPaintingMinigame.gameObject.SetActive(true);
    }

    public void OnPaintingFinished(PaintingTable table)
    {
        ExitMinigame();
        StartCoroutine(ReenableTableNextFrame(table));
    }

    public void ExitMinigame()
    {
        if (!isMinigameActive) return;
        isMinigameActive = false;

        playerMovement.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        minigameCanvas.SetActive(false);
        maskPaintingMinigame.gameObject.SetActive(false);
    }

    private IEnumerator ReenableTableNextFrame(PaintingTable table)
    {
        yield return null; // 1 frame buffer
        table.gameObject.SetActive(true);
    }

    public bool IsMinigameActive()
    {
        return isMinigameActive;
    }
}
