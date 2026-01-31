using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    [Header("Ground Settings")]
    [SerializeField] private GameObject groundObject;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Flower Settings")]
    [SerializeField] private GameObject FlowerGroup;
    [SerializeField] private List<GameObject> flowersToSpawn = new List<GameObject>();
    [SerializeField] private float spawnRadius = 0.3f;
    // poppy violet chrysanthemum
    public GameObject poppyPrefab;
    public GameObject violetPrefab;
    public GameObject chrysanthemumPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private int maxAttemptsPerFlower = 50;
    [SerializeField] private float checkHeight = 10f;

    private Collider groundCollider;
    private Mesh groundMesh;
    private bool isSpawning = false;

    void Start()
    {
        Dependencies.Instance.RegisterDependency<FlowerSpawner>(this);
        if (groundObject == null)
        {
            Debug.LogError("No ground object assigned!");
            return;
        }

        groundCollider = groundObject.GetComponent<Collider>();
        if (groundCollider == null)
        {
            Debug.LogError("Ground object has no Collider!");
            return;
        }

        StartSpawning();
    }

    public void AddFlowerToSpawn(string FlowerType)
    {
        switch (FlowerType)
        {
            case "poppy": flowersToSpawn.Add(poppyPrefab);
                break;
            case "violet":
                flowersToSpawn.Add(violetPrefab);
                break;
            case "chrysanthemum":
                flowersToSpawn.Add(chrysanthemumPrefab);
                break;
        }
        StartSpawning();
    }
    
    void StartSpawning()
    {
        if (flowersToSpawn.Count == 0 || isSpawning)
        {
            return;
        }

        // Start coroutine to spawn flowers one by one
        StartCoroutine(SpawnFlowersRoutine());
    }

    private IEnumerator SpawnFlowersRoutine()
    {
        isSpawning = true;
        while (flowersToSpawn.Count > 0)
        {
            // Find a valid point on the ground
            Vector3 spawnPoint = FindValidSpawnPoint();

            if (spawnPoint != Vector3.zero)
            {
                // Get the first flower from the list
                GameObject flowerPrefab = flowersToSpawn[0];

                // Call the spawn function (empty for you to fill)
                SpawnFlower(flowerPrefab, spawnPoint);

                // Remove the flower from the list
                flowersToSpawn.RemoveAt(0);
            }

            // Wait one frame to prevent infinite loop if no valid points
            yield return null;
        }

        isSpawning = false;
    }
    
    Vector3 FindValidSpawnPoint()
    {
        // Get ground bounds for random point generation
        Bounds groundBounds = groundCollider.bounds;

        for (int i = 0; i < maxAttemptsPerFlower; i++)
        {
            // Generate random point within ground bounds
            Vector3 randomPoint = new Vector3(
                Random.Range(groundBounds.min.x, groundBounds.max.x),
                groundBounds.max.y + checkHeight, // Start above ground
                Random.Range(groundBounds.min.z, groundBounds.max.z)
            );

            // Raycast down to find ground surface
            Ray ray = new Ray(randomPoint, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, checkHeight * 2f, groundLayerMask))
            {
                // Check if we hit our specific ground object
                if (hit.collider.gameObject == groundObject || hit.collider.transform.IsChildOf(groundObject.transform))
                {
                    Vector3 surfacePoint = hit.point;

                    // Check if there are any other objects at this point
                    if (!HasCollisionsAtPoint(surfacePoint))
                    {
                        return surfacePoint;
                    }
                }
            }
        }

        // No valid point found after max attempts
        return Vector3.zero;
    }

    bool HasCollisionsAtPoint(Vector3 point)
    {
        // Check for any colliders in the spawn radius
        Collider[] colliders = Physics.OverlapSphere(point, spawnRadius);

        foreach (Collider collider in colliders)
        {
            // Skip the ground itself
            if (collider.gameObject == groundObject || collider.transform.IsChildOf(groundObject.transform))
                continue;

            // Found another object at this point
            return true;
        }

        return false;
    }

    void SpawnFlower(GameObject flowerPrefab, Vector3 position)
    {
        Instantiate(flowerPrefab, position, Quaternion.identity,FlowerGroup.transform);
    }
}
