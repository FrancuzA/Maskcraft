using System;
using UnityEngine;

public class FlowerScript : MonoBehaviour, IInteractable
{
    public string flowerType;
    public int flowerHP = 100;
    private int currentHP = 100;
    public int damage = 10;
    public int flowerValue;

    private void Start()
    {
        currentHP = flowerHP;
    }
    public void Interact()
    {
        DamageFlower();
    }

    private void DamageFlower()
    {
        currentHP -= damage;
        if (currentHP <= 0) DestroyFlower();
    }

    private void DestroyFlower()
    {
        Dependencies.Instance.GetDependancy<FlowerSpawner>().AddFlowerToSpawn(flowerType);
        AddResources();
        Destroy(gameObject);
    }

    private void AddResources()
    {
        int amount = flowerValue + Inventory.instance.GetResources(flowerType);
        Inventory.instance.SetResources(flowerType,amount);
    }
}
