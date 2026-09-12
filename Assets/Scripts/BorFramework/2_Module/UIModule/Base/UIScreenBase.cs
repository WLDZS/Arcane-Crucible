using System.Collections.Generic;

namespace BorFramework
{
    public abstract class UIScreenBase : IUIElement
    {
        internal readonly Stack<UIWindowBase> WindowStack = new();

        public virtual EUILayer Layer => EUILayer.Screen;
        public abstract string Id { get; }

        public abstract void OnOpen();
        public virtual void OnPause() { }
        public virtual void OnResume() { }
        public abstract void OnClose();
        public abstract void OnDestroy();
    }
}
