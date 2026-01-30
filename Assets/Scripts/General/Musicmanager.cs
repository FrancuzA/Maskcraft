using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class Musicmanager : MonoBehaviour
{
    private void Awake()
    {
        Dependencies.Instance.RegisterDependency<Musicmanager>(this);
    }


    public void PlaySound()
    {

    }
}
