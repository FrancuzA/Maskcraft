using UnityEngine;

public class PaintingTable : MonoBehaviour, IInteractable
{
    [Header("Setup")]
    public GameObject maskPrefab;                  // Prefab maski 3D z punktami PaintablePoint
    public Transform spawnOffsetFromCamera;       // opcjonalny offset (np. 2m przed kamerą)
    public float maskDistance = 2f;               // dystans od kamery

    private GameObject currentMaskInstance;

    public void Interact()
    {
        if (MinigameManager.Instance == null)
        {
            Debug.LogError("❌ MinigameManager not found!");
            return;
        }

        // usuń starą maskę jeśli istnieje
        if (currentMaskInstance != null)
            Destroy(currentMaskInstance);

        // stwórz maskę przed kamerą gracza
        Camera playerCam = Camera.main;
        Vector3 spawnPos = playerCam.transform.position + playerCam.transform.forward * maskDistance;
        Quaternion spawnRot = Quaternion.LookRotation(-playerCam.transform.forward); // patrzy w stronę gracza
        currentMaskInstance = Instantiate(maskPrefab, spawnPos, spawnRot);

        // podłącz do minigierki
        MaskPaintingMinigame minigame = MinigameManager.Instance.maskPaintingMinigame;
        if (minigame != null)
        {
            minigame.maskModel = currentMaskInstance.transform;

            // wypełnij listę punktów
            minigame.points.Clear();
            PaintablePoint[] points = currentMaskInstance.GetComponentsInChildren<PaintablePoint>();
            foreach (var p in points)
                minigame.points.Add(p);

            // start minigierki
            MinigameManager.Instance.EnterMinigame("MaskPainting");
        }

        Debug.Log($"✅ Interacted with {gameObject.name} → Mask Painting Minigame started");
    }
}
