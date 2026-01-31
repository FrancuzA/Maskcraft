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
    private Color flowerColor = Color.green;

    public Action OnMinigameEnd; // callback dla PaintingTable

    // ================= INIT =================
    public void SetResource(string flower)
    {
        switch (flower)
        {
            case "Roza": flowerColor = Color.red; break;
            case "Fiolka": flowerColor = Color.magenta; break;
            case "Slonecznik": flowerColor = Color.yellow; break;
        }

        foreach (var p in points)
        {
            p.paintColor = flowerColor;
        }
    }

    public void InitializeMinigame()
    {
        foreach (var p in points)
            p.ResetPoint();

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
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            PaintablePoint point = hit.collider.GetComponent<PaintablePoint>();
            if (point != null && !point.isPainted)
            {
                point.Paint();
                UpdateProgressText();
            }
        }
    }

    void CheckCompletion()
    {
        foreach (var p in points)
            if (!p.isPainted) return;

        isInitialized = false;
        OnMinigameEnd?.Invoke();
    }

    void UpdateProgressText()
    {
        if (progressText == null) return;

        int painted = 0;
        foreach (var p in points)
            if (p.isPainted) painted++;

        progressText.text = $"{painted}/{points.Count}";
    }
}
