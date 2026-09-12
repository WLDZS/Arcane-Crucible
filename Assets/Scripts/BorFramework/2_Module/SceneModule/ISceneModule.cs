using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace BorFramework
{
    public interface ISceneModule : IModule
    {
        bool IsBusy { get; }
        Scene ActiveScene { get; }

        /// <summary>按资源地址加载场景。Single 切换场景，Additive 叠加场景。</summary>
        UniTask<bool> LoadSceneAsync(string address, LoadSceneMode mode = LoadSceneMode.Single);

        /// <summary>卸载本模块加载的场景，不能卸载最后一个已加载场景。</summary>
        UniTask<bool> UnloadSceneAsync(string address);

        bool SetActiveScene(string address);
        bool TryGetScene(string address, out Scene scene);
    }
}
