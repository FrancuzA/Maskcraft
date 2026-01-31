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
    public int CurrentStep { get; private set; } = 0;

    private string usedWood;
    private string usedMetal;
    private string usedFlower;

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

        // Kolejność
        if ((name == "MetalPour" && CurrentStep < 1) ||
            (name == "MaskPainting" && CurrentStep < 2))
        {
            Debug.LogWarning("Musisz wykonać poprzednią minigrę!");
            return;
        }

        // Zasoby
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

        minigameCanvas.SetActive(true);
        DisableAllMinigames();

        switch (name)
        {
            case "Carving":
                usedWood = GetResourceForMinigame(name);
                carvingMinigame.gameObject.SetActive(true);
                carvingMinigame.SetResource(usedWood);
                carvingMinigame.InitializeMinigame();
                break;

            case "MetalPour":
                usedMetal = GetResourceForMinigame(name);
                metalPourMinigame.gameObject.SetActive(true);
                metalPourMinigame.SetResource(usedMetal);
                metalPourMinigame.InitializeMinigame();
                break;

            case "MaskPainting":
                usedFlower = GetResourceForMinigame(name);
                maskPaintingMinigame.gameObject.SetActive(true);
                maskPaintingMinigame.SetResource(usedFlower);
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

        minigameCanvas.SetActive(false);
        DisableAllMinigames();

        CurrentStep++;

        // JEŚLI skończyliśmy painting → zamykamy cały loop
        if (CurrentStep > 2)
            FinishCraftingLoop();
    }

    void FinishCraftingLoop()
    {
        Debug.Log("🎭 Mask completed! Consuming resources & resetting loop.");

        Inventory.instance.ConsumeResource(usedWood);
        Inventory.instance.ConsumeResource(usedMetal);
        Inventory.instance.ConsumeResource(usedFlower);

        usedWood = null;
        usedMetal = null;
        usedFlower = null;

        CurrentStep = 0;
    }

    void DisableAllMinigames()
    {
        carvingMinigame.gameObject.SetActive(false);
        metalPourMinigame.gameObject.SetActive(false);
        maskPaintingMinigame.gameObject.SetActive(false);
    }

    public bool IsMinigameActive() => isMinigameActive;

    // ================= ZASOBY =================
    public bool HasRequiredResource(string minigame)
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

    public string GetResourceForMinigame(string minigame)
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
