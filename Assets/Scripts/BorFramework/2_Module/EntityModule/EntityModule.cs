using System.Collections.Generic;

namespace BorFramework
{
    public class EntityModule : IEntityModule
    {
        private readonly IMonoModule _monoModule;
        private readonly List<Entity> _entities = new();
        private bool _started;

        public EntityModule(IMonoModule monoModule)
        {
            _monoModule = monoModule;
        }
        
        public void Init()
        {
        }

        public void Start()
        {
            _monoModule.OnUpdate += OnUpdate;
            _started = true;

        }

        public void Stop()
        {
            if (!_started)
                return;

            _monoModule.OnUpdate -= OnUpdate;

            _started = false;
        }

        public void Dispose()
        {
            Stop();

            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                _entities[i].Dispose();
            }

            _entities.Clear();
        }

        public T AddEntity<T>(T entity) where T : Entity
        {
            _entities.Add(entity);

            if (_started)
            {
            }

            return entity;
        }

        private void OnUpdate(float dt)
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].Tick(dt);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            if (_entities.Remove(entity))
            {
                entity.Dispose();
            }
        }
    }
}
