using UnityEngine;

namespace BorFramework
{
    public abstract class CompMono : MonoBehaviour, IComp
    {
        public Entity Entity { get; set; }
    }
}