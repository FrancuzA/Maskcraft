using UnityEngine;

public enum WoodType { Acacia, Willow, Palm }
public enum MetalType { Iron, Gold, Copper }
public enum FlowerType { Poppy, Violet, Chrysanthemum }

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

    // ===== ORDER STATE =====
    

    // ===== PLAYER CRAFT STATE =====
    public WoodType usedWood;
    public MetalType usedMetal;
    public FlowerType usedFlower;

    public int CurrentStep { get; private set; } = 0;
    private bool isMinigameActive = false;

    void Awake()
    {
      Dependencies.Instance.RegisterDependency<MinigameManager>(this);

    }

    // ================= ENTER =================
    public void EnterMinigame(string name)
    {
        if (isMinigameActive) return;

        if ((name == "MetalPour" && CurrentStep < 1) ||
            (name == "MaskPainting" && CurrentStep < 2))
        {
            Debug.Log("Musisz wykonać poprzedni step!");
            return;
        }

        if (!HasRequiredResource(name))
        {
            Debug.Log("Brak zasobów!");
            return;
        }

        isMinigameActive = true;
        playerMovement.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        minigameCanvas.SetActive(true);

        DisableAllMinigames();

        switch (name)
        {
            case "Carving":
                carvingMinigame.gameObject.SetActive(true);
                carvingMinigame.SetResource(GetResourceForMinigame(name));
                carvingMinigame.InitializeMinigame();
                break;

            case "MetalPour":
                metalPourMinigame.gameObject.SetActive(true);
                metalPourMinigame.SetResource(GetResourceForMinigame(name));
                metalPourMinigame.InitializeMinigame();
                break;

            case "MaskPainting":
                maskPaintingMinigame.gameObject.SetActive(true);
                maskPaintingMinigame.SetResource(GetResourceForMinigame(name));
                maskPaintingMinigame.InitializeMinigame();
                
                break;
        }
    }

    // ================= EXIT =================
    public void ExitMinigame()
    {
        isMinigameActive = false;
        CurrentStep++;

        playerMovement.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        minigameCanvas.SetActive(false);
        DisableAllMinigames();

        Debug.Log("Step: " + CurrentStep);
    }



    public void ResetLoop()
    {
        CurrentStep = 0;

        usedWood = default;
        usedMetal = default;
        usedFlower = default;

        
        metalPourMinigame.ResetState();
        maskPaintingMinigame.ResetState();

        Debug.Log("🔄 FULL RESET craftingu");
    }




    void DisableAllMinigames()
    {
        carvingMinigame.gameObject.SetActive(false);
        metalPourMinigame.gameObject.SetActive(false);
        maskPaintingMinigame.gameObject.SetActive(false);
    }

    public bool IsMinigameActive() => isMinigameActive;

    // ================= ORDER =================

    public bool IsOrderCorrect()
    {
        return usedWood == OrderSystem.Instance.currentWood &&
               usedMetal == OrderSystem.Instance.currentMetal &&
               usedFlower == OrderSystem.Instance.currentFlower;
    }



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
