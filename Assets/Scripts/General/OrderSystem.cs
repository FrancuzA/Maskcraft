using FMODUnity;
using UnityEngine;
using static ResourcesTypes;

public class OrderSystem : MonoBehaviour
{
    public static OrderSystem Instance { get; private set; }
    public bool hasActiveOrder = false;
    public string currentMessage;
    public WoodType currentWood;
    public MetalType currentMetal;
    public FlowerType currentFlower;
    public EventReference currentDialogue;
    private OrderList currentOrderList;
    private OrderText currentOrderText;
    private void Awake()
    {
        Dependencies.Instance.RegisterDependency<OrderSystem>(this);
    }
    private void Start()
    {
        
        currentOrderList = Dependencies.Instance.GetDependancy<OrderList>();
        GetNextOrder();
    }

    public void GetNextOrder()
    {
        if (hasActiveOrder) return;
        if (currentOrderList.orders.Count == 0) return;
        currentOrderText = currentOrderList.orders[0];
        currentMessage = currentOrderText.Message;
        currentWood = currentOrderText.woodType;
        currentMetal = currentOrderText.metalType;
        currentFlower = currentOrderText.flowerType;
        currentDialogue = currentOrderText.dialogueSound;


        hasActiveOrder = true;
    }

    public void ClearOrder()
    {
        hasActiveOrder = false;
        GetNextOrder();
    }
}