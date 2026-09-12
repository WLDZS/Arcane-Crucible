using System;
using System.Collections.Generic;
using UnityEngine;

namespace BorFramework
{
    public abstract class Entity
    {
        protected GameObject Go;
        private readonly Dictionary<Type, IComp> _comps = new();
        private readonly Dictionary<Type, ILogic> _logics = new();
        private readonly List<ILogic> _logicOrder = new();

        internal void Tick(float dt)
        {
            foreach (var logic in _logicOrder)
            {
                logic.OnUpdate(dt);
            }
        }

        public virtual void Dispose()
        {
            for (int i = _logicOrder.Count - 1; i >= 0; i--)
            {
                _logicOrder[i].Dispose();
            }

            _logicOrder.Clear();
            _logics.Clear();
        }
        
        protected void AddComp<T>(T comp) where T : IComp
        {
            var type = typeof(T);
            _comps.TryAdd(type, comp);
        }

        protected void AddLogic<T>(T logic) where T : ILogic
        {
            var type = typeof(T);
            if (_logics.TryAdd(type, logic))
            {
                _logicOrder.Add(logic);
                _logicOrder.Sort((left, right) => left.Phase.CompareTo(right.Phase));
            }
        }

        protected T GetComp<T>() where T : class, IComp
        {
            var type = typeof(T);
            if (_comps.TryGetValue(type, out var comp))
            {
                return comp as T;
            }
            return null;
        }

        protected T GetLogic<T>() where T : class, ILogic
        {
            var type = typeof(T);
            if (_logics.TryGetValue(type, out var logic))
            {
                return logic as T;
            }
            return null;
        }
    }
}
