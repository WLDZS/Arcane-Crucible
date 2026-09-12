using System;
using YooAsset;

namespace BorFramework
{
    internal sealed class AssetLease<T> : IAssetLease<T> where T : UnityEngine.Object
    {
        private AssetHandle _handle;
        public T Asset { get; private set; }
        public string Address { get; }
        public bool IsValid => _handle is { IsValid: true } && Asset != null;
        
        public AssetLease(string address, AssetHandle handle, T asset)
        {
            Address = address;
            _handle = handle;
            Asset = asset;
        }
        
        public void Dispose()
        {
            if (_handle == null)
                return;

            if (_handle.IsValid)
                _handle.Release();

            _handle = null;
            Asset = null;
        }
    }
}
