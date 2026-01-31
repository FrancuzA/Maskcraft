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


    public void PlaySound(EventReference SoundRef)
    {
       EventInstance SoundInst =  CreateInstnace(SoundRef);
       SoundInst.start();
       SoundInst.release();
    }

    public EventInstance CreateInstnace(EventReference SoundRef)
    {
        EventInstance SoundInstance = RuntimeManager.CreateInstance(SoundRef);
        return SoundInstance;
    }


}
