using System.Collections;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [Header("Owl Settings")]
    [SerializeField] private GameObject owlPrefab;
    [SerializeField] private Transform[] spawnPoints; // Points where owl appears from horizon
    [SerializeField] private Transform[] dropPoints; // Points where letters are dropped
    [SerializeField] private float flightSpeed = 10f;
    [SerializeField] private float hoverHeight = 30f;
    [SerializeField] private float deliveryInterval = 60f; // Time between deliveries in seconds

    [Header("Letter Settings")]
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private float letterFallSpeed = 5f;

    private bool isDelivering = false;
    private GameObject currentOwl;
    private OrderSystem orderSystem;

    void Start()
    {
        orderSystem = OrderSystem.Instance;

        if (orderSystem == null)
        {
            Debug.LogError("❌ OrderSystem not found!");
            return;
        }

        // Start periodic deliveries
        StartCoroutine(DeliveryRoutine());

        Debug.Log("🦉 Owl Delivery System initialized");
    }

    IEnumerator DeliveryRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(deliveryInterval);

            // Only deliver if there's no active order
            if (!orderSystem.hasActiveOrder && !isDelivering)
            {
                StartDelivery();
            }
        }
    }

    public void StartDelivery()
    {
        if (isDelivering) return;

        StartCoroutine(DeliveryProcess());
    }

    IEnumerator DeliveryProcess()
    {
        isDelivering = true;

        // 1. Generate a new order
        orderSystem.GenerateOrder();

        // 2. Spawn owl at random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Transform dropPoint = dropPoints[Random.Range(0, dropPoints.Length)];

        currentOwl = Instantiate(owlPrefab, spawnPoint.position, Quaternion.identity);

        // 3. Owl flies to drop point
        yield return StartCoroutine(FlyToDropPoint(dropPoint.position));

        // 4. Drop letter
        yield return StartCoroutine(DropLetter(dropPoint.position));

        // 5. Owl flies away
        yield return StartCoroutine(FlyAway());

        isDelivering = false;

        Debug.Log("📬 Letter delivered!");
    }

    IEnumerator FlyToDropPoint(Vector3 targetPosition)
    {
        Vector3 startPos = currentOwl.transform.position;
        Vector3 flyTarget = targetPosition + Vector3.up * hoverHeight;

        float distance = Vector3.Distance(startPos, flyTarget);
        float duration = distance / flightSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            currentOwl.transform.position = Vector3.Lerp(startPos, flyTarget, t);

            // Make owl look at target
            Vector3 direction = (flyTarget - currentOwl.transform.position).normalized;
            if (direction != Vector3.zero)
            {
                currentOwl.transform.rotation = Quaternion.LookRotation(direction);
            }

            yield return null;
        }

        // Hover briefly
        yield return new WaitForSeconds(1f);
    }

    IEnumerator DropLetter(Vector3 dropPosition)
    {
        // Instantiate letter at owl's position
        GameObject letter = Instantiate(letterPrefab, currentOwl.transform.position, Quaternion.identity);

        // Add LetterItem component to the letter
        LetterItem letterItem = letter.AddComponent<LetterItem>();

        // Make letter fall to ground
        float elapsed = 0f;
        Vector3 startPos = letter.transform.position;
        Vector3 groundPos = GetGroundPosition(dropPosition);

        while (elapsed < 2f) // Fall for max 2 seconds
        {
            elapsed += Time.deltaTime;
            float t = elapsed / 2f;

            // Parabolic fall curve
            Vector3 currentPos = Vector3.Lerp(startPos, groundPos, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * 5f; // Arc effect

            letter.transform.position = currentPos;

            // Rotate letter as it falls
            letter.transform.Rotate(Vector3.right * 180f * Time.deltaTime);

            // Check if reached ground
            if (Vector3.Distance(letter.transform.position, groundPos) < 0.5f)
                break;

            yield return null;
        }

        // Ensure letter is on ground
        letter.transform.position = groundPos;
        letter.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 90); // Lay flat on ground

        // Play landing sound/effect here if needed
    }

    IEnumerator FlyAway()
    {
        Vector3 startPos = currentOwl.transform.position;
        Vector3 endPos = startPos + (currentOwl.transform.forward * 100f) + (Vector3.up * 50f);

        float duration = 3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            currentOwl.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        Destroy(currentOwl);
    }

    Vector3 GetGroundPosition(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 50f, Vector3.down, out hit, 100f))
        {
            return hit.point + Vector3.up * 0.1f; // Slightly above ground
        }

        return position;
    }

    // Call this to immediately trigger a delivery (for testing)
    [ContextMenu("Test Delivery")]
    public void TestDelivery()
    {
        StartDelivery();
    }

    void OnDrawGizmosSelected()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.blue;
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 1f);
                    Gizmos.DrawLine(point.position, point.position + point.forward * 5f);
                }
            }
        }

        if (dropPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in dropPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawCube(point.position, Vector3.one * 0.5f);
                }
            }
        }
    }
}