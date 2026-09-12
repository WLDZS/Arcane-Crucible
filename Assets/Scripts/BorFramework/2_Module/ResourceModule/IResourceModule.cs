using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BorFramework
{
    public interface IResourceModule : IModule
    {
        string PackageName { get; }
        EResourceState State { get; }
        UniTask<IAssetLease<T>> LoadAssetAsync<T>(string address) where T : Object;
        IAssetLease<T> LoadAsset<T>(string address) where T : Object;
    }
}
