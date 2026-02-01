using UnityEngine;

public class LetterItem : MonoBehaviour, IInteractable
{
    [Header("Letter Settings")]
    [SerializeField] private GameObject letterVisual;
    

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

        // Add a simple visual if none exists
        if (letterVisual == null)
        {
            CreateDefaultVisual();
        }

        Debug.Log("📮 Letter spawned at " + transform.position);
    }

    void CreateDefaultVisual()
    {
        // Create a simple envelope visual
        GameObject envelope = GameObject.CreatePrimitive(PrimitiveType.Cube);
        envelope.transform.SetParent(transform);
        envelope.transform.localPosition = Vector3.zero;
        envelope.transform.localScale = new Vector3(0.3f, 0.02f, 0.2f);

        
    }

    public void Interact()
    {
        if (isCollected) return;

        CollectLetter();
    }

    void CollectLetter()
    {
        isCollected = true;

        Debug.Log("📬 Letter collected!");

        // Show the letter UI
        if (OrderLetterUI.Instance != null)
        {
            OrderLetterUI.Instance.ShowLetter();
        }

       

        // Destroy the physical letter
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // Optional: Auto-collect when player walks over it
        if (other.CompareTag("Player") && !isCollected)
        {
            CollectLetter();
        }
    }

    // Visual feedback when player looks at it
    void OnMouseEnter()
    {
        if (letterVisual != null)
        {
            // Highlight effect
            Renderer rend = letterVisual.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = Color.yellow;
            }
        }
    }

    void OnMouseExit()
    {
        if (letterVisual != null)
        {
            Renderer rend = letterVisual.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = new Color(0.95f, 0.95f, 0.85f);
            }
        }
    }
}