namespace BorFramework
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate(float dt);
        void OnExit();
    }
}
