using UnityEngine;

public class Terminal : MonoBehaviour, IInteractable
{
    public int rewardGold = 10;
    private BirdController birdController;

    OrderSystem orderSystem;
    MinigameManager minigameManager;
    [System.Obsolete]
    void Start()
    {
        orderSystem=Dependencies.Instance.GetDependancy<OrderSystem>();
        minigameManager=Dependencies.Instance.GetDependancy<MinigameManager>();
        birdController = FindObjectOfType<BirdController>();
        if (birdController == null)
        {
            
        }
    }

    public void Interact()
    {
        Debug.Log("🖐 Terminal interact");

        // Jeśli jest zamówienie i gracz skończył (3 kroki)
        if (orderSystem.hasActiveOrder && minigameManager.CurrentStep >= 3)
        {
            Debug.Log("🔍 Sprawdzam zamówienie...");

            bool success =minigameManager.IsOrderCorrect();

            if (success)
            {
                Debug.Log($"✅ DOBRZE! +{rewardGold} golda");
                Inventory.instance.AddGold(rewardGold);
            }
            else
            {
                Debug.Log("❌ ŹLE! 0 golda");
            }
            Dependencies.Instance.GetDependancy<OrderList>().orders.RemoveAt(0);
            // 1. Resetuj minigry
           minigameManager.ResetLoop();

            // 2. Wyczyść stare zamówienie
           orderSystem.ClearOrder();

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
        else if (orderSystem.hasActiveOrder)
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