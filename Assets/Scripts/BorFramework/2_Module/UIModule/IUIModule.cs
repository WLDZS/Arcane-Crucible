using Cysharp.Threading.Tasks;

namespace BorFramework
{
    public interface IUIModule : IModule
    {
        public void Register<T>(T element) where T : class, IUIElement;

        public UniTask<T> OpenAsync<T>() where T : class, IUIElement;

        public UniTask<T> PushScreenAsync<T>() where T : UIScreenBase;

        public UniTask<T> OpenWindowAsync<T>() where T : UIWindowBase;

        public void Close<T>() where T : class, IUIElement;

        public void Destroy<T>() where T : class, IUIElement;

        public bool TryGet<T>(out T element)
            where T : class, IUIElement;

        public  bool IsOpen<T>() where T : class, IUIElement;

        public bool PopScreen();

        public void Back();
    }
}
