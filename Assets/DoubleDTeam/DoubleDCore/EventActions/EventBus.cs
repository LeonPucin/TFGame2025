using System;
using System.Collections.Generic;

namespace DoubleDCore.EventActions
{
    public class EventBus
    {
        private readonly Dictionary<Type, Delegate> _subscribers = new();

        public void Subscribe<TMark>(Action<TMark> callback)
        {
            var type = typeof(TMark);

            if (_subscribers.TryGetValue(type, out var existingDelegate))
            {
                _subscribers[type] = (Action<TMark>)existingDelegate + callback;
            }
            else
            {
                _subscribers[type] = callback;
            }
        }

        public void Unsubscribe<TMark>(Action<TMark> callback)
        {
            var type = typeof(TMark);

            if (_subscribers.TryGetValue(type, out var existingDelegate) == false)
                return;

            var currentDelegate = (Action<TMark>)existingDelegate;
            currentDelegate -= callback;

            if (currentDelegate == null)
            {
                _subscribers.Remove(type);
            }
            else
            {
                _subscribers[type] = currentDelegate;
            }
        }

        // public void Invoke<TMark>(TMark param)
        // {
        //     var type = typeof(TMark);
        //
        //     if (_subscribers.TryGetValue(type, out var existingDelegate) == false)
        //         return;
        //
        //     if (existingDelegate is Action<TMark> action)
        //     {
        //         action.Invoke(param);
        //     }
        // }

        public void Invoke(object param)
        {
            if (param == null)
                return;

            var type = param.GetType();

            if (_subscribers.TryGetValue(type, out var existingDelegate))
            {
                existingDelegate.DynamicInvoke(param);
            }
        }
    }
}