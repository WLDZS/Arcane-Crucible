using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;
using YooSceneHandle = YooAsset.SceneHandle;

namespace BorFramework
{
    public sealed class SceneModule : ISceneModule
    {
        private readonly IResourceModule _resourceModule;
        private readonly Dictionary<string, YooSceneHandle> _scenes = new();
        private bool _disposed;

        public bool IsBusy { get; private set; }
        public Scene ActiveScene => SceneManager.GetActiveScene();

        public SceneModule(IResourceModule resourceModule)
        {
            _resourceModule = resourceModule;
        }

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
            _disposed = true;
            _scenes.Clear();
            // 随框架整体退出，资源句柄由 ResourceModule 统一销毁。
        }

        public async UniTask<bool> LoadSceneAsync(string address, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (_disposed || IsBusy || _resourceModule == null || string.IsNullOrWhiteSpace(address))
                return false;

            if (mode != LoadSceneMode.Single && mode != LoadSceneMode.Additive)
                return false;

            if (TryGetScene(address, out _))
                return false;

            IsBusy = true;
            while (!_disposed && _resourceModule.State == EResourceState.Initializing)
                await UniTask.Yield();

            if (_disposed || _resourceModule.State != EResourceState.Ready || !YooAssets.IsInitialized
                || !YooAssets.TryGetPackage(_resourceModule.PackageName, out var package))
            {
                IsBusy = false;
                return false;
            }

            var assetInfo = package.GetAssetInfo(address);
            if (!assetInfo.IsValid || !assetInfo.AssetPath.EndsWith(".unity", System.StringComparison.OrdinalIgnoreCase))
            {
                IsBusy = false;
                Debug.LogWarning($"SceneModule场景地址无效：{address}");
                return false;
            }

            var handle = package.LoadSceneAsync(assetInfo, mode);
            await handle;
            IsBusy = false;

            if (_disposed || _resourceModule.State != EResourceState.Ready || !handle.IsValid)
                return false;

            if (handle.Status != EOperationStatus.Succeeded)
            {
                Debug.LogError($"场景加载失败。Address: {address}, Error: {handle.Error}");
                handle.Release();
                return false;
            }

            if (mode == LoadSceneMode.Single)
                _scenes.Clear();

            _scenes[address] = handle;
            return true;
        }

        public async UniTask<bool> UnloadSceneAsync(string address)
        {
            if (_disposed || IsBusy || !TryGetScene(address, out _))
                return false;

            if (SceneManager.loadedSceneCount <= 1)
            {
                Debug.LogWarning("SceneModule不能卸载最后一个场景");
                return false;
            }

            IsBusy = true;
            var operation = _scenes[address].UnloadSceneAsync();
            await operation;
            IsBusy = false;

            if (_disposed)
                return false;

            if (operation.Status != EOperationStatus.Succeeded)
            {
                Debug.LogError($"场景卸载失败。Address: {address}, Error: {operation.Error}");
                return false;
            }

            // YooAsset 在场景卸载时自动释放对应句柄。
            _scenes.Remove(address);
            return true;
        }

        public bool SetActiveScene(string address)
        {
            return !IsBusy && TryGetScene(address, out var scene) && SceneManager.SetActiveScene(scene);
        }

        public bool TryGetScene(string address, out Scene scene)
        {
            scene = default;
            if (_disposed || string.IsNullOrWhiteSpace(address) || !_scenes.TryGetValue(address, out var handle))
                return false;

            if (handle.IsValid && handle.SceneObject.IsValid() && handle.SceneObject.isLoaded)
            {
                scene = handle.SceneObject;
                return true;
            }

            _scenes.Remove(address);
            return false;
        }
    }
}
