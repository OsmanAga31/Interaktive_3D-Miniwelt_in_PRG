using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact()
    {
        // Default implementation does nothing
        Debug.Log("Using " + gameObject.name);
    }
}
