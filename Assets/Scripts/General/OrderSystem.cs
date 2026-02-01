using UnityEngine;
using static ResourcesTypes;

public class OrderSystem : MonoBehaviour
{
    public static OrderSystem Instance { get; private set; }

    public bool hasActiveOrder { get; private set; } = false;
    public WoodType currentWood;
    public MetalType currentMetal;
    public FlowerType currentFlower;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Add this property to OrderSystem.cs
    public bool CanGenerateOrder { get; set; } = true;

    // Modify GenerateOrder method:
    public void GenerateOrder()
    {
        if (hasActiveOrder || !CanGenerateOrder) return;

        currentWood = (WoodType)Random.Range(0, 3);
        currentMetal = (MetalType)Random.Range(0, 3);
        currentFlower = (FlowerType)Random.Range(0, 3);

        hasActiveOrder = true;

        Debug.Log($"📜 New order generated: {currentWood}, {currentMetal}, {currentFlower}");

        // Optional: Play notification sound
        // AudioManager.Instance.Play("NewOrder");
    }

    // Sprawdza czy gracz wykonał zamówienie poprawnie
    public bool CheckOrder()
    {
        bool correct = MinigameManager.Instance.IsOrderCorrect();
        return correct;
    }

    // Czyści zamówienie po weryfikacji
    public void ClearOrder()
    {
        hasActiveOrder = false;
    }
}
