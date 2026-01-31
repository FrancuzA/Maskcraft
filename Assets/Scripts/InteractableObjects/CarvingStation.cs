using UnityEngine;

public class CarvingStation : MonoBehaviour, IInteractable
{
    [Header("Setup")]
    public string promptText = "Carve Mask [E]"; // Optional: show UI prompt
    private bool playerInRange = false;

    void Update()
    {
        // Optional: show/hide interaction prompt UI
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (MinigameManager.Instance == null)
        {
            Debug.LogError("❌ MinigameManager not found! Create empty GameObject with MinigameManager script.");
            return;
        }

        Debug.Log($"✅ Interacted with {gameObject.name} → Starting carving minigame");
        MinigameManager.Instance.EnterMinigame();
    }

    // Optional: visual feedback when player looks at station
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}