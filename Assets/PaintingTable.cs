using UnityEngine;
using System.Collections;

public class PaintingTable : MonoBehaviour, IInteractable
{
    public GameObject maskPrefab;
    public float maskDistanceFromCamera = 1f;

    private GameObject currentMaskInstance;

    public void Interact()
    {
        if (MinigameManager.Instance == null) return;
        if (MinigameManager.Instance.IsMinigameActive()) return;

        // Spawn maski przed kamerą
        Camera cam = Camera.main;
        Vector3 pos = cam.transform.position + cam.transform.forward * maskDistanceFromCamera;
        Quaternion rot = Quaternion.LookRotation(cam.transform.forward);

        currentMaskInstance = Instantiate(maskPrefab, pos, rot);

        MaskPaintingMinigame minigame = MinigameManager.Instance.maskPaintingMinigame;
        minigame.maskModel = currentMaskInstance.transform;

        // Pobierz punkty do malowania
        minigame.points.Clear();
        PaintablePoint[] found = currentMaskInstance.GetComponentsInChildren<PaintablePoint>();
        minigame.points.AddRange(found);

        Debug.Log("🎯 Found paint points: " + found.Length);

        // callback po zakończeniu minigry
        minigame.OnMinigameEnd = OnMinigameEnd;

        // Start minigry
        MinigameManager.Instance.EnterMinigame("MaskPainting");
        minigame.InitializeMinigame();

        // Wyłącz stół dopiero po tym, jak wszystko jest uruchomione
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
