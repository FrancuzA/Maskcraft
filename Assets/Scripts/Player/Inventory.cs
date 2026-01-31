using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    private Dictionary<string, int> resources = new();

    public void SetResources(string resourceType,int amount)
    {
        if (resources.ContainsKey(resourceType))
        {
            resources[resourceType] = amount;
            return;
        }
        resources.Add(resourceType, amount);
    }
    public int GetResources(string resourceType)
    {
        if (resources.TryGetValue(resourceType, out var value))
        {
            return (int)value;
        }

        return default;
    }
    public void ConsumeResource(string resourceType, int amount = 1)
    {
        if (!resources.ContainsKey(resourceType)) return;

        resources[resourceType] -= amount;
        if (resources[resourceType] < 0)
            resources[resourceType] = 0;
    }


}
