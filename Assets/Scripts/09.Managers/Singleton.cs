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
    private static T _instance;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    var currentScene = SceneManager.GetActiveScene().name;

                    // T가 ISceneDependentSingleton을 구현하고 있으며,
                    // 현재 씬에서 생성이 허용되는 경우에만 인스턴스를 생성
                    if (typeof(ISingletonCreatable).IsAssignableFrom(typeof(T)))
                    {
                        var singletonInstance = (T)FindObjectOfType(typeof(T));

                        if (singletonInstance != null)
                        {
                            _instance = singletonInstance;
                        }
                        else if (typeof(T).GetInterface(nameof(ISingletonCreatable)) != null)
                        {
                            var tempObject = new GameObject();
                            var tempInstance = tempObject.AddComponent<T>();
                            if (tempInstance.ShouldBeCreatedInScene(currentScene))
                            {
                                _instance = tempInstance;
                                _instance.gameObject.name = "(singleton) " + typeof(T).ToString();
                                DontDestroyOnLoad(_instance.gameObject);

                                Debug.Log("[Singleton] An instance of " + typeof(T) +
                                    " is needed in the scene, so '" + _instance.gameObject.name +
                                    "' was created with DontDestroyOnLoad.");
                            }
                            else
                            {
                                Destroy(tempObject);
                            }
                        }
                    }
                }

                return _instance;
            }
        }
    }
    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}