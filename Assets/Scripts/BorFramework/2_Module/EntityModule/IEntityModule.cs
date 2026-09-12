namespace BorFramework
{
    public interface IEntityModule : IModule
    {
        T AddEntity<T>(T entity) where T : Entity;
        void RemoveEntity(Entity entity);
    }
}
