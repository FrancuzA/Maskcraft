using UnityEngine;

public class PaintablePoint : MonoBehaviour
{
    public bool isPainted = false;
    public Color paintColor = Color.red;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = Color.white;
    }

    public void Paint()
    {
        if (!isPainted)
        {
            isPainted = true;
            if (rend != null)
                rend.material.color = paintColor;
        }
    }
}
