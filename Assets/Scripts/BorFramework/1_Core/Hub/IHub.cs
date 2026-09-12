using BorFramework;

public interface IHub
{
    public void Init();
    public void RegisterModule<T>(T module) where T : class, IModule;
    public T GetModule<T>() where T : class, IModule; 
    public void InitModules();
    public void StartModules();
    public void StopModules();
    public void DisposeModules();
}
