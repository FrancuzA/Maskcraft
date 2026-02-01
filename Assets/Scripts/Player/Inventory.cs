using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private void Awake() => instance = this;

    private Dictionary<string, int> resources = new();

    // Zasoby
    public void SetResources(string resourceType, int amount)
    {
        resources[resourceType] = amount;
    }

    public int GetResources(string resourceType) => resources.TryGetValue(resourceType, out var val) ? val : 0;

    public void ConsumeResource(string resourceType, int amount = 1)
    {
        if (!resources.ContainsKey(resourceType)) return;
        resources[resourceType] -= amount;
        if (resources[resourceType] < 0) resources[resourceType] = 0;
    }

    // Złoto
    private int gold = 0;

    public int GetGold() => gold;

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"💰 Dodano {amount} gold. Masz teraz: {gold} gold");
    }

    public bool SpendGold(int amount)
    {
        if (gold < amount) return false;
        gold -= amount;
        return true;
    }
}
