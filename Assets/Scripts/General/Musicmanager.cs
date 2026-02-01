using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Debug = UnityEngine.Debug;

public class Musicmanager : MonoBehaviour
{
    public EventReference StepSoundRef;
    private EventInstance StepInstance;
    private Transform playerTransform;
    private void Awake()
    {
        Dependencies.Instance.RegisterDependency<Musicmanager>(this);

    }

    private void Start()
    {   
        playerTransform = Dependencies.Instance.GetDependancy<PlayerMovement>().gameObject.transform;
    }

    public void PlaySound(EventReference SoundRef)
    {
       EventInstance SoundInstance = RuntimeManager.CreateInstance(SoundRef);
       Debug.Log(SoundInstance.isValid());

        RuntimeManager.AttachInstanceToGameObject(SoundInstance, playerTransform.gameObject);
        SoundInstance.start();
       SoundInstance.release();
    }

    public void PlayStep(String GroundType)
    {
        StepInstance = RuntimeManager.CreateInstance(StepSoundRef);
        switch (GroundType)
        {
            case "Grass":
                StepInstance.setParameterByNameWithLabel("FloorType", "Grass");

                break;
            case "Wood":
                StepInstance.setParameterByNameWithLabel("FloorType", "Wood");
                break;
            default:
                StepInstance.setParameterByNameWithLabel("FloorType", "Grass");
                break;
        }
        
            RuntimeManager.AttachInstanceToGameObject(StepInstance, playerTransform.gameObject);
            StepInstance.start();
            StepInstance.release();
    }
}
