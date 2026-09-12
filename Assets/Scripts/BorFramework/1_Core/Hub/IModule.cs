namespace BorFramework
{
    public interface IModule
    {
        public void Init();
        public void Start();
        public void Stop();
        public void Dispose();
    }
}
