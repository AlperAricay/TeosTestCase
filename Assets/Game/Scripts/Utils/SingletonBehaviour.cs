using UnityEngine;

namespace Game.Scripts.Utils
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<T>();
                    if (!_instance) CreateInstance();
                }

                return _instance;
            }

            private set => _instance = value;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<T>();
                if (Instance == null)
                {
                    CreateInstance();
                }
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private static void CreateInstance()
        {
            Debug.LogWarning($"There are no instances of {typeof(T)} in the scene. Instantiating one as a fallback option.");
            var obj = new GameObject(typeof(T).ToString());
            obj.AddComponent<T>();
            _instance = obj.GetComponent<T>();
        }
    }
}
