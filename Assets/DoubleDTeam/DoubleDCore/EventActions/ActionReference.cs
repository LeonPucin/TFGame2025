using System;

namespace DoubleDCore.EventActions
{
    public class ActionReference<TParameter>
    {
        private Action<TParameter> _action;

        public void Invoke(TParameter parameter)
        {
            _action?.Invoke(parameter);
        }

        public void AddListener(Action<TParameter> action)
        {
            _action += action;
        }

        public void RemoveListener(Action<TParameter> action)
        {
            _action -= action;
        }
    }
}