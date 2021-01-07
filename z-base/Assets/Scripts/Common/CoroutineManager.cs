using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }
}
