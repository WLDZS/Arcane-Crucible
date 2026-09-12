namespace BorFramework
{
    public interface ILogModule : IModule
    {
        public void Log(string msg);
        public void Waring(string msg);
        public void Error(string msg);
    }
}