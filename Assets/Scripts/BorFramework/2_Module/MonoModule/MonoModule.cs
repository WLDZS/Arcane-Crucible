using System;

namespace BorFramework
{
    public class MonoModule : IMonoModule
    {
        public event Action<float> OnFixUpdate;
        public event Action<float> OnUpdate;
        public event Action<float> OnLateUpdate;
        
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
            OnFixUpdate = null;
            OnUpdate = null;
            OnLateUpdate = null;
        }
        
        public void DoFixedUpdate(float dt)
        {
            OnFixUpdate?.Invoke(dt);
        }
        
        public void DoUpdate(float dt)
        {
            OnUpdate?.Invoke(dt);
        }
        
        public void DoLateUpdate(float dt)
        {
            OnLateUpdate?.Invoke(dt);
        }
    }
}
