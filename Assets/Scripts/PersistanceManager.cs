using UnityEngine;

public class PersistanceManager : MonoBehaviour
{
    private static PersistanceManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }    
    }
}
