using UnityEngine;

namespace BorFramework
{
    /// <summary>
    /// 血条、单位名称等可重复创建的世界 UI 基类，不参与 Screen 和 Window 导航栈。
    /// </summary>
    public abstract class WorldUIElementBase : MonoBehaviour
    {
        public Transform Target { get; private set; }
        public Vector3 Offset { get; private set; }
        public bool HasTarget => Target != null;

        public virtual void Bind(Transform target, Vector3 offset)
        {
            Target = target;
            Offset = offset;
        }

        public virtual void Unbind()
        {
            Target = null;
            Offset = Vector3.zero;
        }
    }
}
