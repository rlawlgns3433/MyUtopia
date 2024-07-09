using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<SceneIds, SceneController> sceneManagers = new Dictionary<SceneIds, SceneController>();
    //private Dictionary<SceneIds, UIManager> uiManagers = new Dictionary<SceneIds, UIManager>();
    private AnimalManager animalManager;
    private SceneIds currentSceneId;
    public SceneIds CurrentSceneId
    {
        get
        {
            return currentSceneId;
        }
        set
        {
            currentSceneId = value;
            //GetUIManager(currentSceneId).InitializeUI();
            GetSceneController(currentSceneId);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        RegisterSceneManager(SceneIds.WorldSelect, new WorldSelectManager());
        RegisterSceneManager(SceneIds.WorldLandOfHope, new WorldLandOfHopeManager());

        CurrentSceneId = SceneIds.WorldLandOfHope;
    }

    public void RegisterSceneManager(SceneIds sceneName, SceneController sceneManager)
    {
        if (!sceneManagers.ContainsKey(sceneName))
        {
            sceneManagers[sceneName] = sceneManager;
            //uiManagers[sceneName] = new UIManager();
        }
    }

    public SceneController GetSceneController(SceneIds sceneName)
    {
        if (sceneManagers.ContainsKey(sceneName))
        {
            return sceneManagers[sceneName];
        }
        return null;
    }

    //public UIManager GetUIManager(SceneIds sceneName)
    //{
    //    if (uiManagers.ContainsKey(sceneName))
    //    {
    //        return uiManagers[sceneName];
    //    }
    //    return null;
    //}

    private void LoadSceneAsync(int sceneIndex)
    {
        CurrentSceneId = (SceneIds)sceneIndex;
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public AnimalManager GetAnimalManager()
    {
        if(CurrentSceneId == SceneIds.WorldLandOfHope)
        {
            if (animalManager == null)
            {
                animalManager = GameObject.FindWithTag(Tags.AnimalManager).GetComponentInChildren<AnimalManager>();
            }
            return animalManager;
        }
        throw new Exception("AnimalManager is not in the current scene");
    }
}


public class WorldSelectManager : SceneController
{
    public override void Start()
    {
        base.Start();
    }
}

public class WorldLandOfHopeManager : SceneController
{
    public override void Start()
    {
        base.Start();
    }
}

public class SceneController : MonoBehaviour
{
    public virtual void Start()
    {
        Debug.Log("Base SceneController Start");
    }
}