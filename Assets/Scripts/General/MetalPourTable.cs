using UnityEngine;

public class MetalPourTable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (MinigameManager.Instance == null) return;

        MinigameManager.Instance.EnterMinigame("MetalPour");
    }
}
