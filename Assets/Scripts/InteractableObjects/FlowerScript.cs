using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class FlowerScript : MonoBehaviour, IInteractable
{
    [Header("Flower Settings")]
    public string flowerType; // Make sure this matches your resource type names
    public int flowerHP = 100;
    public int damage = 10;
    public int flowerValue = 1;
    private Musicmanager musicManager;
    private int currentHP;
    public EventReference  pickingSound;

    void Start()
    {
        currentHP = flowerHP;
        Debug.Log($"🌼 {flowerType} created at {transform.position}");
        musicManager = Dependencies.Instance.GetDependancy<Musicmanager>();
    }

    public void Interact()
    {
        Debug.Log($"👆 Interacting with {flowerType}");
        DamageFlower();
    }

    void DamageFlower()
    {
        musicManager.PlaySound(pickingSound);
        currentHP -= damage;
        Debug.Log($"💥 {flowerType} took {damage} damage. HP: {currentHP}/{flowerHP}");

        if (currentHP <= 0)
        {
            Debug.Log($"💀 Destroying {flowerType}!");
            DestroyFlower();
        }
    }

    void DestroyFlower()
    {
        // 1. Add resources to inventory BEFORE destroying
        AddToInventory();

        // 2. Get the spawner and respawn a new flower
        var spawner = Dependencies.Instance?.GetDependancy<FlowerSpawner>();
        if (spawner != null)
        {
            Debug.Log($"🔄 Telling spawner to respawn {flowerType}");
            spawner.AddFlowerToSpawn(flowerType);
        }
        else
        {
            Debug.LogError("❌ FlowerSpawner not found in Dependencies!");
        }

        // 3. Destroy this flower
        Debug.Log($"🔥 Destroying flower object");
        Destroy(gameObject);
    }

    void AddToInventory()
    {
        Debug.Log($"💰 Adding {flowerValue} {flowerType} to inventory");

        if (Inventory.instance != null)
        {
            // Get current amount
            int currentAmount = Inventory.instance.GetResources(flowerType);
            Debug.Log($"📊 Current {flowerType} in inventory: {currentAmount}");

            // Calculate new total
            int newAmount = currentAmount + flowerValue;

            // Set the new amount
            Inventory.instance.SetResources(flowerType, newAmount);

            Debug.Log($"✅ Added {flowerValue} {flowerType}. Total now: {newAmount}");
        }
        else
        {
            Debug.LogError("❌ Inventory.instance is null!");

            // For testing without inventory
            Debug.Log($"⚠ TEST MODE: Would add {flowerValue} {flowerType} to inventory");
        }
    }

    // Optional: Visual feedback when damaged
    void OnMouseDown()
    {
        // If you're using mouse clicks for interaction
        Interact();
    }
}