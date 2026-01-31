using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactRange;
    private Quaternion lastRotation;
    public GameObject playerCamera;
    private bool interactableInRange;
    private IInteractable interactable;


    public void Start()
    {
        lastRotation = playerCamera.transform.rotation;
    }
    private void Update()
    {
        if (transform.rotation != lastRotation)
        {
            RotationChanged();
            lastRotation = transform.rotation;
        }

        if (Input.GetKey(KeyCode.Mouse0) && interactableInRange)
        {
            Debug.Log("trying to interact");
            interactable.Interact();
        }

    }

    private void RotationChanged()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hitInfo, interactRange))
        {
            Debug.Log("something in range");
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                Debug.Log("interactable in range");
                interactable = interactObj;
                interactableInRange = true;
                CheckIfStillExist();
            }
            else { interactableInRange = false; }
        }
        else { interactableInRange = false; }
    }

    private void CheckIfStillExist()
    {
        if (interactable == null) interactableInRange = false;
    }
}