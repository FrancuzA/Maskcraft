using System;
using System.Collections;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class OreScript : MonoBehaviour, IInteractable
{
    private int currentHP;
    public int oreHP;
    public int damage;
    public string oreType;
    public int oreValue = 2;
    private Musicmanager musicManager;
    public EventReference miningSound ;
    public void Interact()
    {
        DamageOre();
    }

    private void Start()
    {
        currentHP = oreHP;
    }

    public void DamageOre()
    {
        musicManager.PlaySound(miningSound);
        currentHP -= damage;
        if (currentHP <= 0)DestroyOre();
    }

    private void DestroyOre()
    {
        Dependencies.Instance.GetDependancy<OreSpawner>().AddToQueue(oreType);
        AddResources();
        Destroy(gameObject);
        
    }

    private void AddResources()
    {
        int amountToSet = oreValue + Inventory.instance.GetResources(oreType);
        Inventory.instance.SetResources(oreType,amountToSet);
    }


}
