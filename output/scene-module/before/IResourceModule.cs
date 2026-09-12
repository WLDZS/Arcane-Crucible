using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BorFramework
{
    public interface IResourceModule : IModule
    {
        EResourceState State { get; }
        UniTask<IAssetLease<T>> LoadAssetAsync<T>(string address) where T : Object;
        IAssetLease<T> LoadAsset<T>(string address) where T : Object;
    }
}
