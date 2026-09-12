using System;

namespace BorFramework
{
    /// <summary>
    /// 表示一次资源引用。持有者必须在不再使用资源时释放该对象。
    /// </summary>
    public interface IAssetLease<out T> : IDisposable where T : UnityEngine.Object
    {
        public T Asset { get; }
        public string Address { get; }
        public bool IsValid { get; }
    }
}
