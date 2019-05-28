using UnityEngine;

public class GenericSingletonClass<T> : MonoBehaviour where T : GenericSingletonClass<T>
{
    private static T mInstance;
    public static T instance
    {
        get
        {
            Find();
            return mInstance;
        }
    }
    public bool isPersistant;

    public static void Find()
    {
        if (mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType<T>();
                if (mInstance.isPersistant)
                {
                    T[] j = GameObject.FindObjectsOfType<T>();
                    for (int i = j.Length - 1; i >= 0 ; i--)
                    {
                        if (j[i] != mInstance)
                        {
                            Destroy(j[i].gameObject);
                        }
                    }
                }

                mInstance.Initialize();
                // if (instance.isPersistant)
                // {
                //     if (!instance)
                //     {
                //         instance = this as T;
                //     }
                //     else
                //     {
                //         Destroy(gameObject);
                //     }
                //     DontDestroyOnLoad(gameObject);
                //     Initialize();
                // }
                // else
                // {
                //     instance = this as T;
                // }
            }
    }

    public virtual void Awake()
    {
        Find();
    }

    public virtual void Initialize() { }
}