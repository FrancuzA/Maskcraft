using System;
using UnityEngine;

public class TreeScript : MonoBehaviour, IInteractable
{
    private int currentHp;
    public int treeHP = 100;
    public int damage = 10;
    private TreeStump treeStump;
    public float treeSize;
    public int treeValue = 5;
    public string woodType;

    private void Start()
    {
        currentHp = treeHP;
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hitInfo, treeSize/2))
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
    }

    public void Interact()
    {
        DamageTree();
    }

    private void DamageTree()
    {
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
