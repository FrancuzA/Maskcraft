using UnityEngine;

public class LetterItem : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private float pickupRange = 3f;

    private bool isCollected = false;
    private Collider letterCollider;

    void Start()
    {
        letterCollider = GetComponent<Collider>();
        if (letterCollider == null)
        {
            letterCollider = gameObject.AddComponent<BoxCollider>();
            (letterCollider as BoxCollider).isTrigger = true;
        }

        Debug.Log("📮 Letter spawned at " + transform.position);
    }

    void Update()
    {
        // Optional: Float animation or glow effect
        FloatAnimation();
    }

    void FloatAnimation()
    {
        // Simple floating animation
        float floatHeight = Mathf.Sin(Time.time * 2f) * 0.1f;
        transform.position += new Vector3(0, floatHeight * Time.deltaTime, 0);
    }

    public void Interact()
    {
        if (isCollected) return;
        CollectLetter();
    }

    void OnTriggerEnter(Collider other)
    {
        // Auto-collect when player walks over it
        if (other.CompareTag("Player") && !isCollected)
        {
            CollectLetter();
        }
    }

    void CollectLetter()
    {
        isCollected = true;

        // 1. Tell OrderLetterUI that player now has a letter
        OrderLetterUI.Instance.SetHasLetter(true);

        // 2. Make sure OrderSystem has an active order
        if (!OrderSystem.Instance.hasActiveOrder)
        {
            Debug.LogError("❌ Letter collected but no active order in OrderSystem!");
        }

        Debug.Log("📬 Letter collected! Press TAB to read.");

        // 3. Destroy the physical letter
        Destroy(gameObject);
    }

    // Visual feedback
    void OnMouseEnter()
    {
        // Optional: Highlight effect
    }

    void OnMouseExit()
    {
        // Optional: Remove highlight
    }
}