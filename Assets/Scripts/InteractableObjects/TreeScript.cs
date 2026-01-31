using System;
using UnityEngine;

public class TreeScript : MonoBehaviour, IInteractable
{
    private int treeHP = 100;
    private TreeStump treeStump;
    public float treeSize;

    private void Start()
    {
        treeHP = 100;
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
        Debug.Log("tree hit");
        treeHP -= 10;
        if (treeHP <= 0)
        {
            InformStump();
            
        }
    }

    private void InformStump()
    {
        Debug.Log("stump informed");
        treeStump.StartGrowingTree();
        gameObject.SetActive(false);
    }
}
