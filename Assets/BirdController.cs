using System.Collections;
using UnityEngine;
using FMODUnity;

public class BirdController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator owlAnimator;
    [SerializeField] private string deliveryAnimationName = "OwlDelivery";

    [Header("Delivery Settings")]
    [SerializeField] private Transform[] dropPoints;
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private float letterDropTime = 2.5f; // When in animation to drop letter
    [SerializeField] private EventReference scream;

    private OrderSystem orderSystem;
    private bool isDelivering = false;
    private Vector3 currentDropPoint;
    private Musicmanager musicManager;

    void Start()
    {
        musicManager = Dependencies.Instance.GetDependancy<Musicmanager>();
        orderSystem = OrderSystem.Instance;

        if (owlAnimator == null)
            owlAnimator = GetComponent<Animator>();

        Debug.Log("🦉 Owl Animation Controller ready");

        // Deliver first order at start
        if (orderSystem != null && !orderSystem.hasActiveOrder)
        {
            DeliverFirstOrder();
        }
    }

    void DeliverFirstOrder()
    {
        StartCoroutine(DeliverAfterDelay(3f));
    }

    System.Collections.IEnumerator DeliverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartDelivery();
    }

    public void StartDelivery()
    {
        musicManager.PlaySound(scream);
        if (isDelivering || orderSystem.hasActiveOrder) return;

        Debug.Log("🦉 Starting owl delivery animation");

        // Generate order first
        orderSystem.GenerateOrder();

        // Pick random drop point
        if (dropPoints.Length > 0)
        {
            currentDropPoint = dropPoints[Random.Range(0, dropPoints.Length)].position;
            transform.position = currentDropPoint + new Vector3(-50, 30, 0); // Start position
            transform.LookAt(currentDropPoint);
        }

        // Start animation
        owlAnimator.Play(deliveryAnimationName);
        isDelivering = true;

        // Schedule letter drop during animation
        Invoke("DropLetter", letterDropTime);
    }

    void DropLetter()
    {
        Debug.Log("📨 Dropping letter during animation");

        // Spawn letter
        GameObject letter = Instantiate(letterPrefab, transform.position - Vector3.up * 2, Quaternion.identity);

        // Add LetterItem component if missing
        if (letter.GetComponent<LetterItem>() == null)
        {
            letter.AddComponent<LetterItem>();
        }

        // Make letter fall to ground
        StartCoroutine(DropLetterToGround(letter, currentDropPoint));
    }

        public  System.Collections.IEnumerator DropLetterToGround(GameObject letter, Vector3 targetPos)
    {
        Vector3 startPos = letter.transform.position;
        Vector3 groundPos = GetGroundPosition(targetPos);
        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Arc motion
            Vector3 currentPos = Vector3.Lerp(startPos, groundPos, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * 3f;

            letter.transform.position = currentPos;
            letter.transform.Rotate(Vector3.right * 90f * Time.deltaTime);

            yield return null;
        }

        // Final position
        letter.transform.position = groundPos;
        letter.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 90);

        Debug.Log($"📨 Letter landed at {groundPos}");
    }

    // Animation Event - Called at the end of delivery animation
    public void OnDeliveryComplete()
    {
        isDelivering = false;
        Debug.Log("✅ Owl delivery animation complete");
    }

    Vector3 GetGroundPosition(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 20f, Vector3.down, out hit, 50f))
        {
            return hit.point + Vector3.up * 0.1f;
        }
        return position;
    }

    // Called by Terminal when order is completed
    public void DeliverNextOrder()
    {
        if (!isDelivering && !orderSystem.hasActiveOrder)
        {
            StartCoroutine(DeliverAfterDelay(5f)); // 5 second delay
        }
    }

    [ContextMenu("Test Delivery Now")]
    public void TestDelivery()
    {
        StartDelivery();
    }
}