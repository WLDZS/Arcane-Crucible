using UnityEngine;

namespace BorFramework
{
    public class LogModule : ILogModule
    {
        public void Init()
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void Dispose()
        {
        }
        
        public void Log(string msg)
        {
            Debug.Log(msg);
        }

        public void Waring(string msg)
        {
            Debug.LogWarning(msg);
        }

        public void Error(string msg)
        {
            Debug.LogError(msg);
        }
    }
}
