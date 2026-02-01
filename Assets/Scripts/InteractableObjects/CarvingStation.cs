using UnityEngine;

public class CarvingStation : MonoBehaviour, IInteractable
{
    [Header("Setup")]
    public string promptText = "Carve Mask [E]"; // Optional: show UI prompt
    private bool playerInRange = false;
    private MinigameManager minigameManager;
    private void Start()
    {
        minigameManager = Dependencies.Instance.GetDependancy<MinigameManager>();
    }
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
        minigameManager.EnterMinigame("Carving"); // <-- dodany argument
    }


    // Optional: visual feedback when player looks at station
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}