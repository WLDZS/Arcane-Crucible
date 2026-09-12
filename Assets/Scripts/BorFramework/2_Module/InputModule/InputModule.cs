using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BorFramework
{
    public sealed class InputModule : IInputModule
    {
        private readonly InputActionAsset _inputActions;
        private readonly Dictionary<string, InputAction> _actions = new();

        public InputModule(InputActionAsset inputActions)
        {
            _inputActions = inputActions;
        }

        public void Init()
        {
        }

        public void Start()
        {
            if (_inputActions == null)
            {
                Debug.LogWarning("InputModule未配置InputActionAsset");
                return;
            }

            _inputActions.Enable();
        }

        public void Stop()
        {
            if (_inputActions == null)
                return;

            _inputActions.Disable();
        }

        public void Dispose()
        {
            Stop();
            _actions.Clear();
        }

        public Vector2 ReadVector2(string actionName)
        {
            var action = GetAction(actionName);
            return action == null ? Vector2.zero : action.ReadValue<Vector2>();
        }

        public float ReadFloat(string actionName)
        {
            var action = GetAction(actionName);
            return action == null ? 0f : action.ReadValue<float>();
        }

        public bool IsPressed(string actionName)
        {
            var action = GetAction(actionName);
            return action != null && action.IsPressed();
        }

        public bool WasPressedThisFrame(string actionName)
        {
            var action = GetAction(actionName);
            return action != null && action.WasPressedThisFrame();
        }

        public bool WasReleasedThisFrame(string actionName)
        {
            var action = GetAction(actionName);
            return action != null && action.WasReleasedThisFrame();
        }

        private InputAction GetAction(string actionName)
        {
            if (_inputActions == null || string.IsNullOrWhiteSpace(actionName))
                return null;

            if (_actions.TryGetValue(actionName, out var cachedAction))
                return cachedAction;

            var action = _inputActions.FindAction(actionName, false);
            _actions[actionName] = action;

            if (action == null)
                Debug.LogWarning($"InputModule找不到输入Action：{actionName}");

            return action;
        }
    }
}
