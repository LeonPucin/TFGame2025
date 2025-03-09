using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Tables;

namespace DoubleDCore.Localization.Base
{
    public interface ILocalizationService
    {
        public event Action<string> LanguageChanged;

        public void SetAutoLanguage();

        public bool SetLanguage(string language);

        public string GetLanguage();

        public void SetDefaultLanguage();

        public UniTask<string> GetTranslation(TableEntryReference key, params object[] smartObjects);
    }
}