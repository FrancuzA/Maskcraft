using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeStump : MonoBehaviour
{
    public float timer = 5;
    public GameObject treeBody;
    public Vector3 treeSpawnOffset;
    private bool isGrowwingNewTree = false;


    public void CreateNewTree()
    {
        transform.DestroyAllChildren();
        Debug.Log("growing new tree");
        Instantiate(treeBody, transform.position + treeSpawnOffset, Quaternion.identity, gameObject.transform);
        isGrowwingNewTree = false;
    }

    public void StartGrowingTree()
    {
        Debug.Log("information recieved");
        if(!isGrowwingNewTree)
         StartCoroutine(GrowTree());
    }

    public IEnumerator GrowTree()
    {
        isGrowwingNewTree = true;
        Debug.Log("starting counting");
        yield return new WaitForSecondsRealtime(timer);
        CreateNewTree();
    }
}
