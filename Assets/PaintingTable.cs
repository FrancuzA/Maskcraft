using UnityEngine;

public class PaintingTable : MonoBehaviour, IInteractable
{
    public GameObject maskPrefab;
    public float maskDistanceFromCamera = 1f;

    private GameObject currentMaskInstance;

    public void Interact()
    {
        if (MinigameManager.Instance == null) return;
        if (MinigameManager.Instance.IsMinigameActive()) return;

        if (MinigameManager.Instance.CurrentStep < 2)
        {
            Debug.LogWarning("Musisz najpierw ukończyć Carving i Metal Pour!");
            return;
        }

        if (!MinigameManager.Instance.HasRequiredResource("MaskPainting"))
        {
            Debug.LogWarning("Nie masz żadnego kwiatka w ekwipunku!");
            return;
        }

        string flower = MinigameManager.Instance.GetResourceForMinigame("MaskPainting");

        Camera cam = Camera.main;
        Vector3 pos = cam.transform.position + cam.transform.forward * maskDistanceFromCamera;
        Quaternion rot = Quaternion.LookRotation(cam.transform.forward);

        currentMaskInstance = Instantiate(maskPrefab, pos, rot);

        MaskPaintingMinigame minigame = MinigameManager.Instance.maskPaintingMinigame;
        minigame.maskModel = currentMaskInstance.transform;

        minigame.points.Clear();
        PaintablePoint[] found = currentMaskInstance.GetComponentsInChildren<PaintablePoint>();
        minigame.points.AddRange(found);

        minigame.SetResource(flower);

        minigame.OnMinigameEnd = OnMinigameEnd;

        MinigameManager.Instance.EnterMinigame("MaskPainting");
        minigame.InitializeMinigame();

        gameObject.SetActive(false);
    }

    void OnMinigameEnd()
    {
        // kończymy minigrę (to wywoła też reset loopa)
        MinigameManager.Instance.ExitMinigame();

        if (currentMaskInstance != null)
            Destroy(currentMaskInstance);

        currentMaskInstance = null;

        // stół wraca
        gameObject.SetActive(true);
    }
}


