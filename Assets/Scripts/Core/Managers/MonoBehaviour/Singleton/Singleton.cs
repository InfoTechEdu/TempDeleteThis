using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance 
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = (T)FindObjectOfType(typeof(T));

                return instance;
            }
        }
    }
}
