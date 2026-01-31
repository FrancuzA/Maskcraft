using UnityEngine;

public class MetalPourTable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (MinigameManager.Instance == null)
        {
            Debug.LogError("❌ MinigameManager not found!");
            return;
        }

        Debug.Log($"✅ Interacted with {gameObject.name} → Starting Metal Pour Minigame");
        MinigameManager.Instance.EnterMinigame("MetalPour"); // <-- dodany argument
    }

}