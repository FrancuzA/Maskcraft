using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class TreeScript : MonoBehaviour, IInteractable
{
    private int currentHp;
    public int treeHP = 100;
    public int damage = 10;
    private TreeStump treeStump;
    public float treeSize;
    public int treeValue = 5;
    public string woodType;
    private Musicmanager musicManager;
    public EventReference  cuttingSound;
    private void Start()
    {
        currentHp = treeHP;

        // Zamiast Raycasta, który nie trafia w krzywe drzewa:
        treeStump = GetComponentInParent<TreeStump>();

        if (treeStump == null)
        {
            Debug.LogError("B£¥D: Nie znaleziono TreeStump u rodzica obiektu: " + gameObject.name);
        }

        musicManager = Dependencies.Instance.GetDependancy<Musicmanager>();
    }

    public void Interact()
    {
        DamageTree();
    }

    private void DamageTree()
    {
        musicManager.PlaySound(cuttingSound);
        currentHp -= damage;
        if (currentHp <= 0)
        {
            InformStump();
            
        }
    }

    private void InformStump()
    {
        AddResources();
        treeStump.StartGrowingTree();
        gameObject.SetActive(false);
    }

    private void AddResources()
    {
        int amountToSet = treeValue + Inventory.instance.GetResources(woodType);
        Inventory.instance.SetResources(woodType,amountToSet);
    }
}
