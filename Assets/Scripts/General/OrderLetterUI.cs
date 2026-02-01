using TMPro;
using UnityEngine;

public class OrderLetterUI : MonoBehaviour
{
    public static OrderLetterUI Instance;

    public GameObject panel;
    public TMP_Text letterText;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void ShowLetter()
    {
        if (!OrderSystem.Instance.hasActiveOrder)
            return;

        var o = OrderSystem.Instance;

        letterText.text =
    $@"Dear Mask Maker,

I would like to order a ceremonial mask made of:

Wood: {o.currentWood}
Metal: {o.currentMetal}
Flower: {o.currentFlower}

Sincerely,
A Mysterious Client";

        panel.SetActive(true);

         Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("📄 Letter UI shown");
    }

    public void CloseLetter()
    {
        panel.SetActive(false);

      Time.timeScale = 1f; 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("📄 Letter UI closed");
    }


}
