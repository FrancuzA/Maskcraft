using System.Collections;
using UnityEngine;

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

    // 0 = Carving, 1 = MetalPour, 2 = Painting
    private int currentStep = 0;

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

        // Sprawdzenie kolejności
        if ((name == "MetalPour" && currentStep < 1) ||
            (name == "MaskPainting" && currentStep < 2))
        {
            Debug.LogWarning("Musisz wykonać poprzednią minigrę!");
            return;
        }

        // Sprawdzenie zasobów
        if (!HasRequiredResource(name))
        {
            Debug.LogWarning($"Nie masz wymaganych zasobów do {name}");
            return;
        }

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
                carvingMinigame.SetResource(GetResourceForMinigame(name)); // ustawia teksturę
                carvingMinigame.InitializeMinigame();
                break;

            case "MetalPour":
                metalPourMinigame.gameObject.SetActive(true);
                metalPourMinigame.SetResource(GetResourceForMinigame(name)); // ustawia sprite strumienia
                metalPourMinigame.InitializeMinigame();
                break;

            case "MaskPainting":
                maskPaintingMinigame.gameObject.SetActive(true);
                maskPaintingMinigame.SetResource(GetResourceForMinigame(name)); // ustawia kolor kwiatu
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
        currentStep++; // po ukończeniu minigry przechodzimy do kolejnego kroku
    }

    void DisableAllMinigames()
    {
        carvingMinigame.gameObject.SetActive(false);
        metalPourMinigame.gameObject.SetActive(false);
        maskPaintingMinigame.gameObject.SetActive(false);
    }

    // ================= painting hook =================
    public void OnPaintingFinished(PaintingTable table)
    {
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

    // ================= Zasoby =================
    private bool HasRequiredResource(string minigame)
    {
        switch (minigame)
        {
            case "Carving":
                return Inventory.instance.GetResources("acacia") > 0 ||
                       Inventory.instance.GetResources("willow") > 0 ||
                       Inventory.instance.GetResources("palm") > 0;
            case "MetalPour":
                return Inventory.instance.GetResources("iron") > 0 ||
                       Inventory.instance.GetResources("gold") > 0 ||
                       Inventory.instance.GetResources("copper") > 0;
            case "MaskPainting":
                return Inventory.instance.GetResources("poppy") > 0 ||
                       Inventory.instance.GetResources("violet") > 0 ||
                       Inventory.instance.GetResources("chrysanthemum") > 0;
        }
        return false;
    }

    private string GetResourceForMinigame(string minigame)
    {
        switch (minigame)
        {
            case "Carving":
                if (Inventory.instance.GetResources("acacia") > 0) return "acacia";
                if (Inventory.instance.GetResources("willow") > 0) return "willow";
                return "palm";
            case "MetalPour":
                if (Inventory.instance.GetResources("iron") > 0) return "iron";
                if (Inventory.instance.GetResources("gold") > 0) return "gold";
                return "copper";
            case "MaskPainting":
                if (Inventory.instance.GetResources("poppy") > 0) return "poppy";
                if (Inventory.instance.GetResources("violet") > 0) return "violet";
                return "chrysanthemum";
        }
        return null;
    }
}
