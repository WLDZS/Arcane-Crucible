using System;

namespace BorFramework
{
    public interface IEventModule : IModule
    {
        void Subscribe<TEvent>(Action<TEvent> listener) where TEvent : struct, IEvent;
        void Unsubscribe<TEvent>(Action<TEvent> listener) where TEvent : struct, IEvent;
        void Publish<TEvent>(TEvent eventData) where TEvent : struct, IEvent;
    }
}
