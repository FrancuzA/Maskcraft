using UnityEngine;

public class PaintablePoint : MonoBehaviour
{
    public bool isPainted { get; private set; } = false;
    public Color paintColor = Color.green;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        ResetPoint();
    }

    public void Paint()
    {
        if (isPainted) return; // NIE malujemy ju¿ pomalowanego punktu
        isPainted = true;
        if (rend != null)
            rend.material.color = paintColor;
    }

    public void ResetPoint()
    {
        isPainted = false;
        if (rend != null)
            rend.material.color = Color.red;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isPainted ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}