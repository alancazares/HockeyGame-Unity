using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Singleton<T> //prevents to create objects that aren't meant to be singletons
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
        //set { } we don't want anyone to set this class so we remove it, protecting while sharing externally
    }

    public static bool IsInitialized //helpfull to chech weather the instance exists without cheking explicitly for null
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("[Singleton] Trying to instanciate a second instance of a singleton class.");
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
