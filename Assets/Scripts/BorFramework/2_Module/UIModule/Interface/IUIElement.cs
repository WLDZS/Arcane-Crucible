namespace BorFramework
{
    public interface IUIElement
    {
        string Id { get; }
        EUILayer Layer { get; }

        void OnOpen();
        void OnClose();
        void OnDestroy();
    }
}
