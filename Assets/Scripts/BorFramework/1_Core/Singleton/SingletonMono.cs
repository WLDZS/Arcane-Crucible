using UnityEngine;

namespace BorFramework
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        private static T _instance;
        public static T Ins => _instance;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
