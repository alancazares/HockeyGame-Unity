using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    //what level the game is currently in   ** Done
    //load and unload levels                ** Done
    //keep track of the game state
    //Generate other persistent systems     ** Done

    public GameObject[] SystemPrefabs;

    List<GameObject> _instancedSystemPrefabs;
    List<AsyncOperation> _loadOperations;


    string _currentLevelName = string.Empty;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();



        LoadLevel("Hockey");
    }

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
            //dispatch message
            //transition between scenes
        }
        Debug.Log("Load Complete");

    }
    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Unload Operation Complete");
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for(int i = 0; i< SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if(ao == null) {
            Debug.LogError("[GameManager] Unable to load level" + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        _currentLevelName = levelName;

    }

    public void UnLoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to Unload level" + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for(int i = 0; i<_instancedSystemPrefabs.Count; ++i)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear(); //make sure things are being destroyed
    }
}
