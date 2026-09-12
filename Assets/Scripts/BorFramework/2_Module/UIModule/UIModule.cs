using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BorFramework
{
    public sealed class UIModule : IUIModule
    {
        private readonly Dictionary<Type, IUIElement> _elements = new();
        private readonly HashSet<Type> _opened = new();
        private readonly Stack<UIScreenBase> _screenStack = new();
        public UIRoot UIRoot { get; private set; }

        public void Init()
        {
            var go = new GameObject("UIRoot");
            UnityEngine.Object.DontDestroyOnLoad(go);

            UIRoot = go.AddComponent<UIRoot>();
            UIRoot.BuildLayers();
        }

        public void Start()
        {
        }

        public void Stop()
        {
            while (_screenStack.Count > 0)
                CloseTopScreen();

            foreach (var element in _elements.Values)
                CloseElement(element);
        }

        public void Dispose()
        {
            Stop();

            foreach (var element in _elements.Values)
                element.OnDestroy();

            _elements.Clear();

            if (UIRoot != null)
                UnityEngine.Object.Destroy(UIRoot.gameObject);

            UIRoot = null;
        }

        public void Register<T>(T element) where T : class, IUIElement
        {
            if (element == null || _elements.ContainsKey(typeof(T)))
                return;

            _elements.Add(typeof(T), element);
        }

        public UniTask<T> OpenAsync<T>() where T : class, IUIElement
        {
            if (!TryGet<T>(out var element))
            {
                Debug.LogWarning($"UI 尚未注册：{typeof(T).Name}");
                return UniTask.FromResult<T>(null);
            }

            if (element is UIScreenBase screen)
                OpenScreen(screen);
            else if (element is UIWindowBase window)
                OpenWindow(window);
            else
                OpenElement(element);

            return UniTask.FromResult(element);
        }

        public UniTask<T> PushScreenAsync<T>() where T : UIScreenBase
        {
            if (!TryGet<T>(out var screen) || !OpenScreen(screen))
                return UniTask.FromResult<T>(null);

            return UniTask.FromResult(screen);
        }

        public UniTask<T> OpenWindowAsync<T>() where T : UIWindowBase
        {
            if (!TryGet<T>(out var window) || !OpenWindow(window))
                return UniTask.FromResult<T>(null);

            return UniTask.FromResult(window);
        }

        public void Close<T>() where T : class, IUIElement
        {
            if (!TryGet<T>(out var element))
                return;

            if (element is UIScreenBase screen)
            {
                if (_screenStack.Count > 0 && _screenStack.Peek() == screen)
                    PopScreen();

                return;
            }

            if (element is UIWindowBase window)
            {
                CloseWindow(window);
                return;
            }

            CloseElement(element);
        }

        public void Destroy<T>() where T : class, IUIElement
        {
            if (!TryGet<T>(out var element))
                return;

            Close<T>();
            if (_opened.Contains(element.GetType()))
                return;

            element.OnDestroy();
            _elements.Remove(typeof(T));
        }

        public bool TryGet<T>(out T element) where T : class, IUIElement
        {
            if (_elements.TryGetValue(typeof(T), out var value))
            {
                element = value as T;
                return element != null;
            }

            element = null;
            return false;
        }

        public bool IsOpen<T>() where T : class, IUIElement
        {
            return _opened.Contains(typeof(T));
        }

        public bool PopScreen()
        {
            if (!CloseTopScreen())
                return false;

            if (_screenStack.Count > 0)
                _screenStack.Peek().OnResume();

            return true;
        }

        public void Back()
        {
            if (_screenStack.Count == 0)
                return;

            var screen = _screenStack.Peek();
            if (screen.WindowStack.Count > 0)
            {
                CloseElement(screen.WindowStack.Pop());
                return;
            }

            PopScreen();
        }

        private bool OpenScreen(UIScreenBase screen)
        {
            if (_opened.Contains(screen.GetType()))
                return false;

            if (_screenStack.Count > 0)
                _screenStack.Peek().OnPause();

            _screenStack.Push(screen);
            OpenElement(screen);
            return true;
        }

        private bool OpenWindow(UIWindowBase window)
        {
            if (_screenStack.Count == 0 || _opened.Contains(window.GetType()))
                return false;

            _screenStack.Peek().WindowStack.Push(window);
            OpenElement(window);
            return true;
        }

        private void CloseWindow(UIWindowBase window)
        {
            if (_screenStack.Count == 0)
                return;

            var windowStack = _screenStack.Peek().WindowStack;
            if (windowStack.Count == 0 || windowStack.Peek() != window)
                return;

            windowStack.Pop();
            CloseElement(window);
        }

        private bool CloseTopScreen()
        {
            if (_screenStack.Count == 0)
                return false;

            var screen = _screenStack.Pop();
            while (screen.WindowStack.Count > 0)
                CloseElement(screen.WindowStack.Pop());

            CloseElement(screen);
            return true;
        }

        private void OpenElement(IUIElement element)
        {
            if (_opened.Add(element.GetType()))
                element.OnOpen();
        }

        private void CloseElement(IUIElement element)
        {
            if (_opened.Remove(element.GetType()))
                element.OnClose();
        }
    }
}
