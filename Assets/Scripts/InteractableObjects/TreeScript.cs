using System;
using UnityEngine;

public class TreeScript : MonoBehaviour, IInteractable
{
    private int treeHP = 100;
    private TreeStump treeStump;
    private float treeSize;

    private void Start()
    {
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
        treeHP -= 10;
        if (treeHP <= 0)
        {
            InformStump();
            Destroy(gameObject);
        }
    }

    private void InformStump()
    {
        treeStump.StartGrowingTree();
    }
}
