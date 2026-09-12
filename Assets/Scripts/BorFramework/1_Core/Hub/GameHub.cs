using System;
using System.Collections.Generic;

namespace BorFramework
{
    public class GameHub : Singleton<GameHub>, IHub
    {
        private Dictionary<Type, IModule> _modules;
        private List<IModule> _moduleOrder;

        public bool IsInitialized { get; private set; }
        public bool IsRunning { get; private set; }
        
        public void Init()
        {
            if (IsInitialized)
                return;

            _modules = new Dictionary<Type, IModule>();
            _moduleOrder = new List<IModule>();
            IsInitialized = true;
            IsRunning = false;
        }

        public void RegisterModule<T>(T module) where T : class, IModule
        {
            var type = typeof(T);
            if (_modules.TryAdd(type, module))
            {
                _moduleOrder.Add(module);
            }
        }

        public T GetModule<T>() where T : class, IModule
        {
            var type = typeof(T);
            _modules.TryGetValue(type, out IModule module);
            return module as T;
        }

        public void InitModules()
        {
            foreach (var module in _moduleOrder)
            {
                module.Init();
            }
        }

        public void StartModules()
        {
            if (!IsInitialized || IsRunning)
                return;

            foreach (var module in _moduleOrder)
            {
                module.Start();
            }

            IsRunning = true;
        }

        public void StopModules()
        {
            if (!IsInitialized || !IsRunning)
                return;

            for (int i = _moduleOrder.Count - 1; i >= 0; i--)
            {
                _moduleOrder[i].Stop();
            }

            IsRunning = false;
        }

        public void DisposeModules()
        {
            if (!IsInitialized)
                return;

            for (int i = _moduleOrder.Count - 1; i >= 0; i--)
            {
                _moduleOrder[i].Dispose();
            }

            _modules.Clear();
            _moduleOrder.Clear();
            IsInitialized = false;
            IsRunning = false;
        }
    }
}
