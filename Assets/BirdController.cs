using System.Collections;
using UnityEngine;
using FMODUnity;

public class BirdController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator owlAnimator;

    [Header("Delivery Points")]
    [SerializeField] private GameObject letterPrefab;

    [Header("Sound")]
    [SerializeField] private EventReference scream;

    private OrderSystem orderSystem;
    private Musicmanager musicManager;
    private bool isDelivering = false;
    

    void Start()
    {

        Dependencies.Instance.RegisterDependency<BirdController>(this);
        musicManager = Dependencies.Instance.GetDependancy<Musicmanager>();
        orderSystem = OrderSystem.Instance;

        // Upewnij się że mamy Animatora
        if (owlAnimator == null)
            owlAnimator = GetComponent<Animator>();


        startDelivery();

            
    
    
    }


    public void startDelivery()
    {
        isDelivering = true;    
        owlAnimator.SetTrigger("Fly");
    }

    void DropLetter()
    {
        Debug.Log("📨 Upuszczam list podczas animacji!");

        GameObject letter = Instantiate(letterPrefab, transform.position, Quaternion.identity);



    }

    public void EndDelivery()
    {
        isDelivering=false;
    }




    // Wołane przez Terminal
    public void DeliverNextOrder()
    {
       

        if (!isDelivering  || !orderSystem.hasActiveOrder)
        {
           
            startDelivery();
        }
    }

    public void Scream()
    {
        musicManager.PlaySound(scream);
    }
}