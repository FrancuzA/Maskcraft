using UnityEngine;

public class MetalPourTable : MonoBehaviour, IInteractable
{
    private MinigameManager minigameManager;
    private void Start()
    {
        minigameManager = Dependencies.Instance.GetDependancy<MinigameManager>();
    }
    public void Interact()
    {
        if (minigameManager == null) return;

        minigameManager.EnterMinigame("MetalPour");
    }
}
