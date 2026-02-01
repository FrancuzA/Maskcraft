using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactRange;
    public Animator HandAnim;
    public GameObject playerCamera;
    private bool interactableInRange;
    private IInteractable interactable;
    private Quaternion lastRotation;


    public void Start()
    {
        lastRotation = playerCamera.transform.rotation;
    }
    private void Update()
    {
        if (transform.rotation != lastRotation)
        {
            lastRotation = transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && interactableInRange)
        {
            if (interactable != null)
            {
                interactable.Interact();
                HandleAnimation();
            }
                
        }

    }

    private void HandleAnimation()
    {
        string t = Dependencies.Instance.GetDependancy<PlayerMovement>().currentItem;
        switch (t)
        {
            case "axe":
                HandAnim.SetTrigger("UseAxe");
                break;
            case "pickaxe":
                HandAnim.SetTrigger("UsePickaxe");
                break;
            case null: return;
        }
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hitInfo, interactRange))
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