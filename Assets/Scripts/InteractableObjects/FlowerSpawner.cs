using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    [Header("Flower Prefabs")]
    public GameObject poppyPrefab;
    public GameObject violetPrefab;
    public GameObject chrysanthemumPrefab;

    [Header("Spawn Zones")]
    [Tooltip("Add Box Colliders to define where flowers can spawn")]
    [SerializeField] private List<BoxCollider> spawnZones = new List<BoxCollider>();

    [Header("Spawn Settings")]
    [SerializeField] private int flowersPerType = 10;
    [SerializeField] private float minDistanceBetweenFlowers = 1.5f;

    private Dictionary<string, GameObject> flowerPrefabDict = new Dictionary<string, GameObject>();
    private GameObject flowerParent;
    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        Debug.Log("🚀 FlowerSpawner Starting...");

        // Setup Dependencies
        if (Dependencies.Instance == null)
        {
            GameObject depsGO = new GameObject("DependencyManager");
            depsGO.AddComponent<Dependencies>();
            Debug.Log("📦 Created Dependencies manager");
        }

        Dependencies.Instance.RegisterDependency<FlowerSpawner>(this);
        Debug.Log("✅ Registered with Dependencies");

        // Create parent for all flowers
        flowerParent = new GameObject("Flowers");
        Debug.Log("📁 Created flower parent");

        // Initialize flower dictionary
        InitializeFlowerDictionary();

        // Check if we have spawn zones
        if (spawnZones.Count == 0)
        {
            Debug.LogWarning("⚠ No spawn zones assigned! Creating a default one...");
            CreateDefaultSpawnZone();
        }

        // Spawn all flowers
        SpawnAllFlowers();

        Debug.Log("🏁 FlowerSpawner ready!");
    }

    void InitializeFlowerDictionary()
    {
        flowerPrefabDict.Clear();

        // Add each flower type to the dictionary
        if (poppyPrefab != null)
        {
            flowerPrefabDict.Add("poppy", poppyPrefab);
            Debug.Log("➕ Added poppy prefab");

            // Make sure the prefab has correct flower type
            var flowerScript = poppyPrefab.GetComponent<FlowerScript>();
            if (flowerScript != null)
                flowerScript.flowerType = "poppy";
        }

        if (violetPrefab != null)
        {
            flowerPrefabDict.Add("violet", violetPrefab);
            Debug.Log("➕ Added violet prefab");

            var flowerScript = violetPrefab.GetComponent<FlowerScript>();
            if (flowerScript != null)
                flowerScript.flowerType = "violet";
        }

        if (chrysanthemumPrefab != null)
        {
            flowerPrefabDict.Add("chrysanthemum", chrysanthemumPrefab);
            Debug.Log("➕ Added chrysanthemum prefab");

            var flowerScript = chrysanthemumPrefab.GetComponent<FlowerScript>();
            if (flowerScript != null)
                flowerScript.flowerType = "chrysanthemum";
        }

        Debug.Log($"📚 Total flower types: {flowerPrefabDict.Count}");
    }

    void CreateDefaultSpawnZone()
    {
        // Create a default spawn zone in the center of the scene
        GameObject defaultZone = new GameObject("DefaultSpawnZone");
        defaultZone.transform.position = new Vector3(0, 0, 0);

        BoxCollider boxCollider = defaultZone.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(20, 10, 20); // 20x20 area

        spawnZones.Add(boxCollider);
        Debug.Log("📦 Created default spawn zone (20x20) at center");
    }

    void SpawnAllFlowers()
    {
        spawnedPositions.Clear();
        Debug.Log($"🌱 Starting to spawn {flowersPerType} of each flower type...");

        // Spawn each type of flower
        foreach (var flowerType in flowerPrefabDict.Keys)
        {
            GameObject prefab = flowerPrefabDict[flowerType];
            int flowersSpawned = 0;
            int attempts = 0;
            int maxAttempts = flowersPerType * 10; // Prevent infinite loop

            while (flowersSpawned < flowersPerType && attempts < maxAttempts)
            {
                // Get a random position inside spawn zones
                Vector3 spawnPosition = GetRandomPositionInZones();

                // Check if position is valid (not too close to other flowers)
                if (spawnPosition != Vector3.zero && IsPositionValid(spawnPosition))
                {
                    SpawnSingleFlower(prefab, spawnPosition);
                    spawnedPositions.Add(spawnPosition);
                    flowersSpawned++;

                    if (flowersSpawned % 5 == 0) // Log every 5 flowers
                        Debug.Log($"🌼 Spawned {flowerType} #{flowersSpawned}");
                }

                attempts++;
            }

            Debug.Log($"✅ Spawned {flowersSpawned} {flowerType} flowers");
        }

        Debug.Log($"🎉 Total flowers spawned: {spawnedPositions.Count}");
    }

    Vector3 GetRandomPositionInZones()
    {
        if (spawnZones.Count == 0)
        {
            Debug.LogError("❌ No spawn zones!");
            return Vector3.zero;
        }

        // Pick a random spawn zone
        BoxCollider zone = spawnZones[Random.Range(0, spawnZones.Count)];

        // Get random point inside the box collider (in local space)
        Vector3 localPoint = new Vector3(
            Random.Range(-zone.size.x / 2, zone.size.x / 2),
            Random.Range(-zone.size.y / 2, zone.size.y / 2),
            Random.Range(-zone.size.z / 2, zone.size.z / 2)
        );

        // Convert to world space
        Vector3 worldPoint = zone.transform.TransformPoint(localPoint);

        // Raycast down to find the ground/terrain
        return GetGroundPosition(worldPoint);
    }

    Vector3 GetGroundPosition(Vector3 point)
    {
        // Start raycast from above the point
        Vector3 rayStart = point + Vector3.up * 50f;

        RaycastHit hit;
        if (Physics.Raycast(rayStart, Vector3.down, out hit, 100f))
        {
            // Found ground! Return position slightly above it
            return hit.point + Vector3.up * 0.1f;
        }

        // If no ground found, return original point (might be on flat surface)
        return point;
    }

    bool IsPositionValid(Vector3 position)
    {
        // Check distance from other spawned flowers
        foreach (Vector3 existingPos in spawnedPositions)
        {
            float distance = Vector3.Distance(position, existingPos);
            if (distance < minDistanceBetweenFlowers)
            {
                return false; // Too close to another flower
            }
        }

        return true;
    }

    void SpawnSingleFlower(GameObject prefab, Vector3 position)
    {
        // Random rotation for variety
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        // Instantiate the flower
        GameObject newFlower = Instantiate(prefab, position, rotation, flowerParent.transform);

        Debug.Log($"📍 Spawned {prefab.name} at {position}");
    }

    // This is called when a flower is destroyed
    public void AddFlowerToSpawn(string flowerType)
    {
        Debug.Log($"🔄 Respawning flower: {flowerType}");

        if (!flowerPrefabDict.ContainsKey(flowerType))
        {
            Debug.LogError($"❌ Unknown flower type: {flowerType}");
            return;
        }

        GameObject prefab = flowerPrefabDict[flowerType];

        // Try to find a valid position (try up to 30 times)
        for (int i = 0; i < 30; i++)
        {
            Vector3 spawnPosition = GetRandomPositionInZones();

            if (spawnPosition != Vector3.zero && IsPositionValid(spawnPosition))
            {
                SpawnSingleFlower(prefab, spawnPosition);
                spawnedPositions.Add(spawnPosition);
                Debug.Log($"✅ Respawned {flowerType}");
                return;
            }
        }

        Debug.LogWarning($"⚠ Couldn't find good spot for {flowerType}, spawning anyway");

        // Last resort: spawn without distance check
        Vector3 lastResortPosition = GetRandomPositionInZones();
        if (lastResortPosition != Vector3.zero)
        {
            SpawnSingleFlower(prefab, lastResortPosition);
            spawnedPositions.Add(lastResortPosition);
        }
    }

    // Debug/Testing functions
    void Update()
    {
        // Press F to spawn more flowers
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("🎮 Manual spawn triggered");
            AddFlowerToSpawn("poppy");
            AddFlowerToSpawn("violet");
            AddFlowerToSpawn("chrysanthemum");
        }

        // Press R to reset all flowers
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("🔄 Resetting all flowers");
            ResetAllFlowers();
        }
    }

    void ResetAllFlowers()
    {
        // Destroy all existing flowers
        if (flowerParent != null)
        {
            foreach (Transform child in flowerParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Clear spawned positions
        spawnedPositions.Clear();

        // Spawn new flowers
        SpawnAllFlowers();
    }

    // Draw gizmos in Scene view to visualize spawn zones
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        foreach (BoxCollider zone in spawnZones)
        {
            if (zone != null)
            {
                // Draw wireframe of the box collider
                Gizmos.matrix = zone.transform.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, zone.size);
            }
        }
    }

    // Editor helper buttons
    [ContextMenu("Spawn Flowers Now")]
    public void SpawnFlowersEditor()
    {
        SpawnAllFlowers();
    }

    [ContextMenu("Clear All Flowers")]
    public void ClearFlowers()
    {
        if (flowerParent != null)
        {
            foreach (Transform child in flowerParent.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        spawnedPositions.Clear();
        Debug.Log("🧹 Cleared all flowers");
    }
}