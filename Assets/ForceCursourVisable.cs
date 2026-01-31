using UnityEngine;

public class ForceCursorVisible : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("✅ Cursor forced VISIBLE");
    }
}