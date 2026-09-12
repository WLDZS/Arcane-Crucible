using UnityEngine;

namespace BorFramework
{
    public interface IInputModule : IModule
    {
        Vector2 ReadVector2(string actionName);
        float ReadFloat(string actionName);
        bool IsPressed(string actionName);
        bool WasPressedThisFrame(string actionName);
        bool WasReleasedThisFrame(string actionName);
    }
}
