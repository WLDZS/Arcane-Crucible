using BorFramework;

namespace BorFramework
{
    public abstract class UIPopupBase : IUIElement
    {
        public virtual EUILayer Layer => EUILayer.Popup;
        public abstract string Id { get; }
        public abstract void OnOpen();
        public abstract void OnClose();
        public abstract void OnDestroy();
    }
}