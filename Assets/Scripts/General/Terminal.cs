using UnityEngine;

public class Terminal : MonoBehaviour, IInteractable
{
    public int rewardGold = 10;
    private BirdController birdController;

    [System.Obsolete]
    void Start()
    {
        birdController = FindObjectOfType<BirdController>();
        if (birdController == null)
        {
            
        }
    }

    public void Interact()
    {
        Debug.Log("🖐 Terminal interact");

        // Jeśli jest zamówienie i gracz skończył (3 kroki)
        if (OrderSystem.Instance.hasActiveOrder && MinigameManager.Instance.CurrentStep >= 3)
        {
            Debug.Log("🔍 Sprawdzam zamówienie...");

            bool success = MinigameManager.Instance.IsOrderCorrect();

            if (success)
            {
                Debug.Log($"✅ DOBRZE! +{rewardGold} golda");
                Inventory.instance.AddGold(rewardGold);
            }
            else
            {
                Debug.Log("❌ ŹLE! 0 golda");
            }

            // 1. Resetuj minigry
            MinigameManager.Instance.ResetLoop();

            // 2. Wyczyść stare zamówienie
            OrderSystem.Instance.ClearOrder();

            // 3. Wyczyść stary list z UI
            if (OrderLetterUI.Instance != null)
            {
                OrderLetterUI.Instance.SetHasLetter(false);
            }

            // 4. OD RAZU WOŁAJ SOWĘ po NOWE zamówienie!
            if (birdController != null)
            {
                Debug.Log("🦉 Wołam sowę po nowe zamówienie!");
                birdController.DeliverNextOrder();
            }
            else
            {
                Debug.LogError("❌ Nie ma sowy do zawołania!");
            }
        }
        // Jeśli jest zamówienie ale gracz nie skończył
        else if (OrderSystem.Instance.hasActiveOrder)
        {
            Debug.Log("📄 Pokazuję aktualny list");
            if (OrderLetterUI.Instance != null)
            {
                OrderLetterUI.Instance.ShowLetter();
            }
        }
        // Jeśli nie ma zamówienia
        else
        {
            Debug.Log("📭 Nie ma aktywnego zamówienia");

            // Możesz od razu zawołać sowę
            if (birdController != null)
            {
                Debug.Log("🦉 Wołam sowę bo nie ma zamówienia!");
                birdController.DeliverNextOrder();
            }
        }
    }
}