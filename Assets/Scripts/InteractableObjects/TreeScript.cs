using UnityEngine;

public class TreeScript : MonoBehaviour, IInteractable
{
    private int treeHP = 100;
    private TreeStump treeStump;
    public void Interact()
    {
        
    }

    private void DamageTree()
    {
        treeHP -= 10;
        
    }

    private void InformStump()
    {
        
    }
}
