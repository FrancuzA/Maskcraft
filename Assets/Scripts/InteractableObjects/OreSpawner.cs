using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OreSpawner : MonoBehaviour
{
    public List<String> oreToSpawn = new List<String>();
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> oreTypes; // copper iron gold
    public bool isSpawning = false;

    public void AddToQueue(string oreType)
    {
        oreToSpawn.Add(oreType);
        if(!isSpawning) TryToSpawn();
    }
    public void TryToSpawn()
    {
        if (oreToSpawn.Count > 0)
        {
            switch (oreToSpawn[0])
            {
                case "copper":
                    Timer(oreTypes[0]);
                    break;
                case "iron":
                    Timer(oreTypes[1]);
                    break;
                case "gold":
                    Timer(oreTypes[2]);
                    break;
            }
        }
    }
    private IEnumerator Timer(GameObject ore)
    {
        isSpawning = true;
        yield return new WaitForSecondsRealtime(50);
        GameObject spawnPoint = GetSpawnPoint();
        Spawn(spawnPoint.transform,ore);
    }

    private void Spawn(Transform parent, GameObject ore)
    {
        Instantiate(ore, parent.position, Quaternion.identity, parent);
        isSpawning = false; 
        TryToSpawn();
    }

    private GameObject GetSpawnPoint()
    {
        int randomNumber = GenerateRandomNumber();
        while (spawnPoints[randomNumber].transform.childCount > 0)
        {
             randomNumber = GenerateRandomNumber();
        }
        return spawnPoints[randomNumber];
    }

    private int GenerateRandomNumber()
    {
        int number = Random.Range(0, spawnPoints.Count);
        return number;
    }
}
