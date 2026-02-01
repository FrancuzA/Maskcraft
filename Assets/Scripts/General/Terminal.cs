using UnityEngine;

public class Terminal : MonoBehaviour, IInteractable
{
    public int rewardGold = 10;
    private BirdController birdController;

    void Start()
    {
        birdController = FindObjectOfType<BirdController>();
    }

    public void Interact()
    {
        Debug.Log("🖐 Terminal interact");

        // If there's an active order and player has completed all steps
        if (OrderSystem.Instance.hasActiveOrder && MinigameManager.Instance.CurrentStep >= 3)
        {
            Debug.Log("🔍 Verifying order");

            bool success = MinigameManager.Instance.IsOrderCorrect();

            if (success)
            {
                Debug.Log($"✅ SUCCESS +{rewardGold} gold");
                Inventory.instance.AddGold(rewardGold);
            }
            else
            {
                Debug.Log("❌ FAIL 0 gold");
            }

            // Reset everything
            MinigameManager.Instance.ResetLoop();
            OrderSystem.Instance.ClearOrder();

            // Remove current letter from player
            if (OrderLetterUI.Instance != null)
            {
                OrderLetterUI.Instance.SetHasLetter(false);
                Debug.Log("🗑️ Old letter removed from player");
            }

            // Notify bird controller to deliver next order
            if (birdController != null)
            {
                birdController.DeliverNextOrder();
            }
        }
        else if (OrderSystem.Instance.hasActiveOrder)
        {
            // Player wants to read current letter
            if (OrderLetterUI.Instance != null)
            {
                OrderLetterUI.Instance.ShowLetter();
            }
        }
        else
        {
            Debug.Log("📭 No active order. Wait for owl!");
        }
    }
}