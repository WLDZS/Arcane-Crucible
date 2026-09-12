using System;
using System.Collections.Generic;

namespace BorFramework
{
    public sealed class StateMachine
    {
        private readonly Dictionary<Type, State> _states = new();

        public State CurrentState { get; private set; }
        public bool IsRunning => CurrentState != null;

        public bool AddState(State state)
        {
            if (state == null)
                return false;

            var stateType = state.GetType();
            if (_states.ContainsKey(stateType))
                return false;

            state.Bind(this);
            _states.Add(stateType, state);
            return true;
        }

        public bool ChangeState<T>() where T : State
        {
            if (!_states.TryGetValue(typeof(T), out var nextState))
                return false;

            if (ReferenceEquals(CurrentState, nextState))
                return false;

            CurrentState?.OnExit();
            CurrentState = nextState;
            CurrentState.OnEnter();
            return true;
        }

        public bool IsCurrent<T>() where T : State
        {
            return CurrentState is T;
        }

        public void Update(float dt)
        {
            CurrentState?.OnUpdate(dt);
        }

        public void Stop()
        {
            if (CurrentState == null)
                return;

            CurrentState.OnExit();
            CurrentState = null;
        }

        public void Clear()
        {
            Stop();
            _states.Clear();
        }
    }
}
