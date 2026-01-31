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

        // Sprawdzenie kolejności (czy gracz zrobił carving i metal pour)
        if (MinigameManager.Instance.CurrentStep < 2)
        {
            Debug.LogWarning("Musisz najpierw ukończyć Carving i Metal Pour!");
            return;
        }

        // Sprawdzenie, czy gracz ma kwiat w ekwipunku
        if (!MinigameManager.Instance.HasRequiredResource("MaskPainting"))
        {
            Debug.LogWarning("Nie masz żadnego kwiatka w ekwipunku!");
            return;
        }

        // Pobierz typ kwiatka
        string flower = MinigameManager.Instance.GetResourceForMinigame("MaskPainting");

        // Spawn maski przed kamerą
        Camera cam = Camera.main;
        Vector3 pos = cam.transform.position + cam.transform.forward * maskDistanceFromCamera;
        Quaternion rot = Quaternion.LookRotation(cam.transform.forward);

        currentMaskInstance = Instantiate(maskPrefab, pos, rot);

        // Pobierz minigame i ustaw model maski
        MaskPaintingMinigame minigame = MinigameManager.Instance.maskPaintingMinigame;
        minigame.maskModel = currentMaskInstance.transform;

        // Pobierz punkty do malowania
        minigame.points.Clear();
        PaintablePoint[] found = currentMaskInstance.GetComponentsInChildren<PaintablePoint>();
        minigame.points.AddRange(found);

        // Ustaw kolor kwiatu
        minigame.SetResource(flower);

        Debug.Log("🎯 Found paint points: " + found.Length + " | Flower: " + flower);

        // Callback po zakończeniu minigry
        minigame.OnMinigameEnd = OnMinigameEnd;

        // Start minigry przez manager
        MinigameManager.Instance.EnterMinigame("MaskPainting");
        minigame.InitializeMinigame();

        // Wyłącz stół
        gameObject.SetActive(false);
    }

    void OnMinigameEnd()
    {
        // 1. Zakończ minigre (canvas + gracz)
        MinigameManager.Instance.ExitMinigame();

        // 2. Usuń maskę
        if (currentMaskInstance != null)
            Destroy(currentMaskInstance);

        currentMaskInstance = null;

        // 3. Włącz stół ponownie przez MinigameManager
        if (MinigameManager.Instance != null)
            MinigameManager.Instance.OnPaintingFinished(this);
    }
}
