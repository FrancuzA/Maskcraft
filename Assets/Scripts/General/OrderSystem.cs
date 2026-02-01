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

    // Generuje nowe zamówienie (losowe)
    public void GenerateOrder()
    {
        if (hasActiveOrder) return; // nie generuj, jeśli już jest aktywne

        currentWood = (WoodType)Random.Range(0, 3);
        currentMetal = (MetalType)Random.Range(0, 3);
        currentFlower = (FlowerType)Random.Range(0, 3);

        hasActiveOrder = true;

        Debug.Log($"📜 Nowe zamówienie: {currentWood}, {currentMetal}, {currentFlower}");
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
