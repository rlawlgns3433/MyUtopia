using UnityEngine;
using UnityEngine.SceneManagement;
public interface ISingletonCreatable
{
    bool ShouldBeCreatedInScene(string sceneName);
}
/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour, ISingletonCreatable
{
    public static T _instance;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var sceneName = SceneManager.GetActiveScene().name;

                        if (typeof(ISingletonCreatable).IsAssignableFrom(typeof(T)))
                        {
                            var existingInstance = (T)FindObjectOfType(typeof(T));
                            if (existingInstance != null)
                            {
                                _instance = existingInstance;
                            }
                            else
                            {
                                var tempObject = new GameObject();
                                var tempInstance = tempObject.AddComponent<T>();

                                if (tempInstance.ShouldBeCreatedInScene(sceneName))
                                {
                                    _instance = tempInstance;
                                    DontDestroyOnLoad(_instance.gameObject);
                                }
                                else
                                {
                                    Destroy(tempObject);
                                }
                            }
                        }
                    }
                }
            }
            return _instance;
        }
    }

    public static void DestroySingleton()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }
    public static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public virtual void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}