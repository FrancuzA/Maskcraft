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
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hitInfo, treeSize))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out TreeStump stump))
            {
                treeStump= stump;
            }
        }
        else
        {
            Debug.Log("no stump found");
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
