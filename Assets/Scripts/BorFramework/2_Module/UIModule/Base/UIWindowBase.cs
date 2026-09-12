namespace BorFramework
{
    public abstract class UIWindowBase : IUIElement
    {
        public virtual EUILayer Layer => EUILayer.Window;
        public abstract string Id { get; }
        public abstract void OnOpen();
        public abstract void OnClose();
        public abstract void OnDestroy();
    }
}
