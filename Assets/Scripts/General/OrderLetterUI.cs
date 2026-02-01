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

    private bool isUIOpen = false;
    private bool hasLetter = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        panel.SetActive(false);
    }

    void Update()
    {
        // Toggle letter UI with TAB
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

        Debug.Log($"📬 Player {(has ? "now has" : "no longer has")} a letter");
    }

    public void ShowLetter()
    {
        if (!OrderSystem.Instance.hasActiveOrder || !hasLetter)
            return;

        var o = OrderSystem.Instance;

        letterText.text = FormatOrderText(o.currentWood, o.currentMetal, o.currentFlower);
        panel.SetActive(true);
        isUIOpen = true;

        // Optional: Pause game
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("📄 Letter UI shown (TAB)");
    }

    public void CloseLetter()
    {
        panel.SetActive(false);
        isUIOpen = false;

        // Resume game
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("📄 Letter UI closed");
    }

    string FormatOrderText(WoodType wood, MetalType metal, FlowerType flower)
    {
        return $@"Dear Mask Maker,

I would like to order a ceremonial mask made of:

Wood: {wood}
Metal: {metal}
Flower: {flower}

Sincerely,
A Mysterious Client";
    }

    // Quick method to check if player can read letter
    public bool CanReadLetter()
    {
        return hasLetter && OrderSystem.Instance.hasActiveOrder;
    }
}

