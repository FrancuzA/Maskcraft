using UnityEngine;
using TMPro;
using System;
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
    private bool hasEnded = false;

    public Action OnMinigameEnd;

    public void InitializeMinigame()
    {
        if (points.Count == 0)
        {
            Debug.LogError("❌ NO PAINTABLE POINTS ASSIGNED!");
            return;
        }

        hasEnded = false;
        isInitialized = true;

        foreach (var p in points)
            p.ResetPoint();

        UpdateProgressText();

        Debug.Log("🎮 MaskPainting initialized with " + points.Count + " points");
    }

    void Update()
    {
        if (!isInitialized || hasEnded) return;

        HandleRotation();
        HandlePainting();
        CheckCompletion();
    }

    void HandleRotation()
    {
        if (maskModel == null) return;

        float rotation = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) rotation = -manualRotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) rotation = manualRotationSpeed * Time.deltaTime;

        maskModel.Rotate(Vector3.up, rotation + autoRotationSpeed * Time.deltaTime);
    }

    void HandlePainting()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            PaintablePoint point = hit.collider.GetComponent<PaintablePoint>();
            if (point != null && !point.isPainted)
            {
                point.Paint();
                UpdateProgressText();
                Debug.Log("🟢 Painted: " + point.name);
            }
        }
    }

    void CheckCompletion()
    {
        foreach (var p in points)
        {
            if (!p.isPainted)
                return;
        }

        // WSZYSTKIE POMALOWANE
        hasEnded = true;
        isInitialized = false;

        Debug.Log("🏁 ALL POINTS PAINTED - END MINIGAME");

        OnMinigameEnd?.Invoke();
    }

    void UpdateProgressText()
    {
        if (progressText == null) return;

        int painted = 0;
        foreach (var p in points)
            if (p.isPainted) painted++;

        progressText.text = $"Points: {painted}/{points.Count}";
    }
}
