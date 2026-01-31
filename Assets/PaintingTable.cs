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

        gameObject.SetActive(false); // wyłącz stół

        Camera cam = Camera.main;
        Vector3 pos = cam.transform.position + cam.transform.forward * maskDistanceFromCamera;
        Quaternion rot = Quaternion.LookRotation(cam.transform.forward);

        currentMaskInstance = Instantiate(maskPrefab, pos, rot);

        MaskPaintingMinigame minigame = MinigameManager.Instance.maskPaintingMinigame;
        minigame.maskModel = currentMaskInstance.transform;

        // ZBIERZ POINTY Z MASKI
        minigame.points.Clear();
        PaintablePoint[] found = currentMaskInstance.GetComponentsInChildren<PaintablePoint>();
        minigame.points.AddRange(found);

        Debug.Log("🎯 Found paint points: " + found.Length);

        minigame.OnMinigameEnd = OnMinigameEnd;

        MinigameManager.Instance.EnterMinigame("MaskPainting");
        minigame.InitializeMinigame();
    }

    void OnMinigameEnd()
    {
        Destroy(currentMaskInstance);
        currentMaskInstance = null;

        MinigameManager.Instance.OnPaintingFinished(this);
    }
}
