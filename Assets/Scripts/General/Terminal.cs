using UnityEngine;

public class Terminal : MonoBehaviour, IInteractable
{
    public int rewardGold = 10;

    public void Interact()
    {
        Debug.Log("🖐 Terminal interact");

        // brak zamówienia → generuj
        if (!OrderSystem.Instance.hasActiveOrder)
        {
            Debug.Log("📄 New order generated");
            OrderSystem.Instance.GenerateOrder();
            OrderLetterUI.Instance.ShowLetter();
            return;
        }

        // są 3 kroki → ocena
        if (MinigameManager.Instance.CurrentStep >= 3)
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

            // reset wszystkiego
            MinigameManager.Instance.ResetLoop();
            OrderSystem.Instance.ClearOrder();
        }
        else
        {
            // w trakcie craftingu → tylko pokaz list
            Debug.Log("📄 Showing letter again");
            OrderLetterUI.Instance.ShowLetter();
        }
    }
}
