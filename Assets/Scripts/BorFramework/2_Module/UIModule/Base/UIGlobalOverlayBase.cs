namespace BorFramework
{
    public abstract class UIGlobalOverlayBase : IUIElement
    {
        public virtual EUILayer Layer => EUILayer.GlobalOverlay;
        public abstract string Id { get; }
        public abstract void OnOpen();
        public abstract void OnClose();
        public abstract void OnDestroy();
    }
}
