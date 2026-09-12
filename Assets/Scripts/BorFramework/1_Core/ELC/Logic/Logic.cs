namespace BorFramework
{
    public abstract class Logic : ILogic
    {
        private int _blockCount;
        private bool _isRunning;

        public virtual ELogicPhase Phase => ELogicPhase.Command;

        public bool IsBlocked => _blockCount > 0;

        public virtual void OnStart() { }

        public void OnUpdate(float dt)
        {
            if (IsBlocked)
            {
                if (_isRunning)
                {
                    OnStop();
                    _isRunning = false;
                }

                return;
            }

            if (!_isRunning)
            {
                OnStart();
                _isRunning = true;
            }

            OnTick(dt);
        }

        protected abstract void OnTick(float dt);

        public virtual void OnStop() { }

        public virtual void Dispose() { }

        public void Block()
        {
            _blockCount++;
        }

        public void UnBlock()
        {
            if (_blockCount > 0)
                _blockCount--;
        }
    }
}
