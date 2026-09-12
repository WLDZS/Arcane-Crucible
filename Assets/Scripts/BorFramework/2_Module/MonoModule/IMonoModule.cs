using System;
namespace BorFramework
{
    public interface IMonoModule : IModule
    {
        public event Action<float> OnFixUpdate;
        public event Action<float> OnUpdate;
        public event Action<float> OnLateUpdate;
        
        public void DoFixedUpdate(float dt);
        public void DoUpdate(float dt);
        public void DoLateUpdate(float dt);
    }
}