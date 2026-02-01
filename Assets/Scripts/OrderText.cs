using FMODUnity;
using UnityEngine;
using static ResourcesTypes;
[CreateAssetMenu(fileName = "OrderInfo", menuName = "MyGame/OrderInfo")]
public class OrderText : ScriptableObject
{
    public string Message;
    public WoodType woodType;
    public MetalType metalType;
    public FlowerType flowerType;
    public EventReference dialogueSound;
}
