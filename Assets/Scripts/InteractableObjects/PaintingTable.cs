using UnityEngine;

public class PaintingTable : MonoBehaviour, IInteractable
{
    public GameObject acaciaMaskPrefab;
    public GameObject acaciaMaskPrefab2;
    public GameObject willowMaskPrefab;
    public GameObject willowMaskPrefab2;
    public GameObject palmMaskPrefab;
    public GameObject palmMaskPrefab2;
    public GameObject maskprefab;
    public GameObject completeMask;
    public float maskDistanceFromCamera = 1f;
    public Transform maskSpawnPoint;
    public GameObject paintingCanvas;

    private MinigameManager minigameManager;
    private GameObject currentMaskInstance;
    private void Start()
    {
        minigameManager= Dependencies.Instance.GetDependancy<MinigameManager>();
    }
    public void Interact()
    {
        
        if (minigameManager.CurrentStep < 2)
        {
            Debug.LogWarning("Musisz najpierw ukończyć Carving i Metal Pour!");
            return;
        }

        if (!minigameManager.HasRequiredResource("MaskPainting"))
        {
            Debug.LogWarning("Nie masz żadnego kwiatka w ekwipunku!");
            return;
        }
        
        string flower = minigameManager.GetResourceForMinigame("MaskPainting");
        switch(minigameManager.usedWood)
        {
            case WoodType.Acacia: maskprefab = acaciaMaskPrefab;
                                  completeMask = acaciaMaskPrefab2;
                break;

            case WoodType.Willow: maskprefab = willowMaskPrefab;
                                   completeMask = willowMaskPrefab2;
                break;
            case WoodType.Palm: maskprefab = palmMaskPrefab;
                                completeMask = palmMaskPrefab2; 
                break;
            default: maskprefab = willowMaskPrefab;
                    completeMask = willowMaskPrefab2;
                break;

        }
        Camera cam = Camera.main;
        Vector3 pos = cam.transform.position + cam.transform.forward * maskDistanceFromCamera;
        Quaternion rot = Quaternion.LookRotation(cam.transform.forward);

        currentMaskInstance = Instantiate(maskprefab, pos, rot);

        MaskPaintingMinigame minigame = minigameManager.maskPaintingMinigame;
        minigame.maskModel = currentMaskInstance.transform;

        minigame.points.Clear();
        PaintablePoint[] found = currentMaskInstance.GetComponentsInChildren<PaintablePoint>();
        minigame.points.AddRange(found);

        minigame.SetResource(flower);

        minigame.OnMinigameEnd = OnMinigameEnd;

        minigameManager.EnterMinigame("MaskPainting");
        minigame.InitializeMinigame();
        paintingCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnMinigameEnd()
    {
        // kończymy minigrę (to wywoła też reset loopa)
        minigameManager.ExitMinigame();
        Instantiate(completeMask, maskSpawnPoint.position, maskSpawnPoint.rotation);
        if (currentMaskInstance != null)
            Destroy(currentMaskInstance);

        currentMaskInstance = null;

        // stół wraca
        gameObject.SetActive(true);
        paintingCanvas.SetActive(false);
    }
}


