using System;
using TMPro;
using UnityEngine;

public class OrderLetterUI : MonoBehaviour
{
    public static OrderLetterUI Instance;

    [Header("UI References")]
    public GameObject panel;
    public TMP_Text letterText;

    [Header("Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    public bool hasLetter = false;

    private bool isUIOpen = false;
    private Musicmanager musicManager;
    

    void Awake()
    {
        Dependencies.Instance.RegisterDependency<OrderLetterUI>(this);
    }

    public void Start()
    {
        musicManager = Dependencies.Instance.GetDependancy<Musicmanager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {

            if (hasLetter && !isUIOpen)
            {
                ShowLetter();
            }
            else if (isUIOpen)
            {
                CloseLetter();
            }
        
        }

        // Auto-close if player no longer has a letter
        if (isUIOpen && !hasLetter)
        {
            CloseLetter();
        }
    }

    // Called when owl delivers a letter or player picks one up
    public void SetHasLetter(bool has)
    {
        hasLetter = has;
      
        if (!has && isUIOpen)
        {
            CloseLetter();
        }

        // If we now have a letter and panel is open but shouldn't be, close it
        if (has && isUIOpen && panel != null && panel.activeSelf)
        {
            
            ShowLetter(); // This will properly set up the letter text
        }
    }

    public void ShowLetter()
    {

        OrderSystem o = Dependencies.Instance.GetDependancy<OrderSystem>();
        Debug.Log(o.currentMessage);
        letterText.text = $"{o.currentMessage}";

        panel.SetActive(true);
        isUIOpen = true;
        if (!o.currentDialogue.IsNull)
        {
            musicManager.PlaySound(o.currentDialogue);
        }
        // Pause game
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseLetter()
    {
        if (!isUIOpen) return;

        if (panel != null)
        {
            panel.SetActive(false);
        }
        isUIOpen = false;

        // Resume game
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}