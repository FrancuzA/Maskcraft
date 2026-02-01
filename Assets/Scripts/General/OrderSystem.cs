using UnityEngine;
using static ResourcesTypes;

public class OrderSystem : MonoBehaviour
{
    public static OrderSystem Instance { get; private set; }
    public bool hasActiveOrder { get; private set; }
    public WoodType currentWood;
    public MetalType currentMetal;
    public FlowerType currentFlower;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void GenerateOrder()
    {
        if (hasActiveOrder) return;

        currentWood = (WoodType)Random.Range(0, 3);
        currentMetal = (MetalType)Random.Range(0, 3);
        currentFlower = (FlowerType)Random.Range(0, 3);

        hasActiveOrder = true;
        Debug.Log($"📜 New order: {currentWood}, {currentMetal}, {currentFlower}");
    }

    public void ClearOrder()
    {
        hasActiveOrder = false;
        Debug.Log("🧹 Order cleared");
    }
}