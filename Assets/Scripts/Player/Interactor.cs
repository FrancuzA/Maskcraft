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
            interactable.Interact();
        }

    }

    private void RotationChanged()
    {
        if (Physics.Raycast(gameObject.transform.position, playerCamera.transform.forward, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactable = interactObj;
                interactableInRange = true;
            }
            else { interactableInRange = false; }
        }
        else { interactableInRange = false; }
    }
}