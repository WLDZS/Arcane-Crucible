using System;
using System.Collections.Generic;

namespace BorFramework
{
    public sealed class EventModule : IEventModule
    {
        private readonly Dictionary<Type, Delegate> _listeners = new();

        public void Init()
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void Dispose()
        {
            _listeners.Clear();
        }

        public void Subscribe<TEvent>(Action<TEvent> listener) where TEvent : struct, IEvent
        {
            if (listener == null)
                return;

            var eventType = typeof(TEvent);
            if (_listeners.TryGetValue(eventType, out var registered))
            {
                _listeners[eventType] = Delegate.Combine(registered, listener);
                return;
            }

            _listeners.Add(eventType, listener);
        }

        public void Unsubscribe<TEvent>(Action<TEvent> listener) where TEvent : struct, IEvent
        {
            if (listener == null)
                return;

            var eventType = typeof(TEvent);
            if (!_listeners.TryGetValue(eventType, out var registered))
                return;

            var remaining = Delegate.Remove(registered, listener);
            if (remaining == null)
                _listeners.Remove(eventType);
            else
                _listeners[eventType] = remaining;
        }

        public void Publish<TEvent>(TEvent eventData) where TEvent : struct, IEvent
        {
            if (!_listeners.TryGetValue(typeof(TEvent), out var registered))
                return;

            (registered as Action<TEvent>)?.Invoke(eventData);
        }
    }
}
