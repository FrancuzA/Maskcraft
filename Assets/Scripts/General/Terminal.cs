using UnityEngine;

public class Terminal : MonoBehaviour, IInteractable
{
    public int rewardGold = 10;

    void Start()
    {
        // Make sure OrderSystem exists
        if (OrderSystem.Instance == null)
        {
            Debug.LogError("❌ OrderSystem not found!");
        }
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
                Debug.Log("✅ SUCCESS +10 gold");
                Inventory.instance.AddGold(rewardGold);
            }
            else
            {
                Debug.Log("❌ FAIL 0 gold");
            }

            // Reset everything
            MinigameManager.Instance.ResetLoop();
            OrderSystem.Instance.ClearOrder();
        }
        else if (OrderSystem.Instance.hasActiveOrder)
        {
            // Show current order letter
            Debug.Log("📄 Showing current order");
            OrderLetterUI.Instance.ShowLetter();
        }
        else
        {
            // No active order - tell player to wait for owl
            Debug.Log("📭 No active order. Wait for the owl delivery!");

            // Optional: Show message UI
            // MessageUI.Instance.ShowMessage("Wait for the owl to deliver your next order!");
        }
    }
}