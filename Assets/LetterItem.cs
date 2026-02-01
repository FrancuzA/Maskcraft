using UnityEngine;

public class LetterItem : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private float pickupRange = 3f;

    private bool isCollected = false;
    private Collider letterCollider;
    private OrderSystem orderSystem;
    void Start()
    {
        letterCollider = GetComponent<Collider>();
        if (letterCollider == null)
        {
            letterCollider = gameObject.AddComponent<BoxCollider>();
            (letterCollider as BoxCollider).isTrigger = true;
        }
        Dependencies.Instance.GetDependancy<OrderSystem>();
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
        Dependencies.Instance.GetDependancy<OrderLetterUI>().hasLetter = true;

        Destroy(gameObject);
    }

}