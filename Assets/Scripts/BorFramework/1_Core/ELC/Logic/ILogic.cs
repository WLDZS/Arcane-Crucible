namespace BorFramework
{
    public interface ILogic
    {
        public ELogicPhase Phase { get; }
        public bool IsBlocked { get; }
        public void Block();
        public void UnBlock();
        public void OnStart();
        public void OnUpdate(float dt);
        public void OnStop();
        public void Dispose();
    }
}
