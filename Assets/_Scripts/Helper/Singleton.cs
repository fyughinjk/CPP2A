using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    static T instance;

    public static T Instance
    {

        get
        {
            instance =FindAnyObjectByType<T>();
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!instance)
        {
            instance = this as T;
            DontDestroyOnLoad(instance);
            return;
        }

        //if code gets down here it means we aren't the first instance
        Destroy(gameObject);
    }
}