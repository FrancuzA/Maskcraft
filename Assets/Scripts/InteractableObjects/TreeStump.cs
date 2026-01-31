using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeStump : MonoBehaviour
{
    private float timer = 50;
    public GameObject treeBody;
    public Vector3 treeSpawnOffset;


    public void CreateNewTree()
    {
        Instantiate(treeBody, transform.position + treeSpawnOffset, Quaternion.identity, gameObject.transform);
    }

    public void StartGrowingTree()
    {
        StartCoroutine(GrowTree());
    }

    public IEnumerator GrowTree()
    {
        yield return new WaitForSecondsRealtime(timer);
        CreateNewTree();
    }
}
