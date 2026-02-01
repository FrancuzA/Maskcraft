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
        Instantiate(treeBody, transform.position + treeSpawnOffset, transform.rotation, gameObject.transform);
        isGrowwingNewTree = false;
    }

    public void StartGrowingTree()
    {
        if(!isGrowwingNewTree)
         StartCoroutine(GrowTree());
    }

    public IEnumerator GrowTree()
    {
        isGrowwingNewTree = true;
        yield return new WaitForSecondsRealtime(timer);
        CreateNewTree();
    }
}
