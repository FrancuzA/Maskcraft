using UnityEngine;

public class PaintablePoint : MonoBehaviour
{
    public bool isPainted { get; private set; } = false;

    // teraz paintColor bêdzie ustawiany dynamicznie
    public Color paintColor { get; private set; } = Color.green;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        ResetPoint();
    }

    /// <summary>
    /// Ustaw kolor dla tego punktu na podstawie kwiatu
    /// </summary>
    public void SetFlowerColor(string flower)
    {
        switch (flower.ToLower())
        {
            case "poppy":
                paintColor = Color.red;
                break;
            case "violet":
                paintColor = new Color(0.5f, 0f, 0.5f); // fioletowy
                break;
            case "chrysanthemum":
                paintColor = Color.yellow;
                break;
            default:
                paintColor = Color.green;
                break;
        }
    }

    public void Paint()
    {
        if (isPainted) return; // NIE malujemy ju¿ pomalowanego punktu
        isPainted = true;
        //deckalon
        gameObject.SetActive(false);
    }

    public void ResetPoint()
    {
        isPainted = false;
        if (rend != null)
            rend.material.color = Color.red; // domyœlny "niepomalowany" kolor
    }


}
