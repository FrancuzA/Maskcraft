using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MaskPaintingMinigame : MonoBehaviour
{
    [Header("References")]
    public Transform maskModel;
    public List<PaintablePoint> points = new List<PaintablePoint>();
    public TMP_Text progressText;

    [Header("Rotation")]
    public float autoRotationSpeed = 20f;
    public float manualRotationSpeed = 50f;

    private bool isInitialized = false;

    public void InitializeMinigame()
    {
        foreach (var p in points)
        {
            p.isPainted = false;
            if (p.GetComponent<Renderer>() != null)
                p.GetComponent<Renderer>().material.color = Color.white;
        }

        isInitialized = true;
        UpdateProgressText();
    }

    void Update()
    {
        if (!isInitialized) return;

        HandleRotation();
        HandlePainting();
        CheckCompletion();
    }

    void HandleRotation()
    {
        float rotation = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) rotation = -manualRotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) rotation = manualRotationSpeed * Time.deltaTime;

        maskModel.Rotate(Vector3.up, rotation + autoRotationSpeed * Time.deltaTime);
    }

    void HandlePainting()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                PaintablePoint point = hit.collider.GetComponent<PaintablePoint>();
                if (point != null)
                {
                    point.Paint();
                    UpdateProgressText();
                }
            }
        }
    }

    void CheckCompletion()
    {
        foreach (var p in points)
        {
            if (!p.isPainted) return; // jeœli choæ jeden nie pomalowany, return
        }

        // wygrana
        isInitialized = false;
        if (MinigameManager.Instance != null)
            MinigameManager.Instance.ExitMinigame();
    }

    void UpdateProgressText()
    {
        if (progressText != null)
        {
            int paintedCount = 0;
            foreach (var p in points) if (p.isPainted) paintedCount++;
            progressText.text = $"Points: {paintedCount}/{points.Count}";
        }
    }
}
