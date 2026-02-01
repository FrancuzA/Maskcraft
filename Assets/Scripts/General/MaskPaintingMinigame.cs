using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using static ResourcesTypes;

public class MaskPaintingMinigame : MonoBehaviour
{
    [Header("References")]
    public Transform maskModel;
    public List<PaintablePoint> points = new List<PaintablePoint>();
    public TMP_Text progressText;
    public EventReference brushSound;
    [Header("Rotation")]
    public float autoRotationSpeed = 20f;
    public float manualRotationSpeed = 50f;

    private bool isInitialized = false;
    private FlowerType usedFlower;
    private Musicmanager musicManager;
    public Action OnMinigameEnd;


    private void Start()
    {
        musicManager = Dependencies.Instance.GetDependancy<Musicmanager>();
    }

    public void SetResource(string flower)
    {
        foreach (var p in points)
            p.SetFlowerColor(flower);

        usedFlower = flower switch
        {
            "poppy" => FlowerType.Poppy,
            "violet" => FlowerType.Violet,
            "chrysanthemum" => FlowerType.Chrysanthemum,
            _ => FlowerType.Poppy
        };
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
                musicManager.PlaySound(brushSound);
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

        // zapisujemy użyty flower
        MinigameManager.Instance.usedFlower = usedFlower;

        OnMinigameEnd?.Invoke();
    }
    public void ResetState()
    {
        isInitialized = false;

        foreach (var p in points)
            p.ResetPoint();

        if (maskModel != null)
            maskModel.rotation = Quaternion.identity;

        UpdateProgressText();
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
