using System;
using UnityEngine.Localization;

namespace DoubleDCore.Localization
{
    public class TextLocalization : IDisposable
    {
        private readonly Action<string> _setter;
        private readonly LocalizedString _translate;

        public TextLocalization(LocalizedString translate, Action<string> setter)
        {
            _setter = setter;
            _translate = translate;
            _translate.StringChanged += OnStringChanged;

            _translate.RefreshString();
        }

        public void Dispose()
        {
            _translate.StringChanged -= OnStringChanged;
        }

        public void InsertParameters(params object[] parameters)
        {
            if (_translate == null)
                return;

            _translate.Arguments = parameters;
            _translate.RefreshString();
        }

        private void OnStringChanged(string value)
        {
            _setter.Invoke(value);
        }
    }
}