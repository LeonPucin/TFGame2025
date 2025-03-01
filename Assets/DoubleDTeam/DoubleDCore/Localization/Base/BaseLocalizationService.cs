using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace DoubleDCore.Localization.Base
{
    public abstract class BaseLocalizationService : ILocalizationService, IDisposable
    {
        public event Action<string> LanguageChanged;

        protected BaseLocalizationService()
        {
            LocalizationSettings.SelectedLocaleChanged += OnLanguageChanged;
        }

        public void Dispose()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLanguageChanged;
        }

        public virtual bool SetLanguage(string language)
        {
            Locale selectedLocale = LocalizationSettings.AvailableLocales.GetLocale(language);

            if (selectedLocale != null)
            {
                LocalizationSettings.SelectedLocale = selectedLocale;
                return true;
            }

            Debug.LogWarning($"Locale with code {language} not found!");
            return false;
        }

        public string GetLanguage()
        {
            return LocalizationSettings.SelectedLocale.Identifier.Code;
        }

        public UniTask<string> GetTranslation(string key, params object[] smartObjects)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedStringAsync(key, smartObjects).Task.AsUniTask();
        }

        public abstract void SetAutoLanguage();

        public abstract void SetDefaultLanguage();

        private void OnLanguageChanged(Locale newLocale)
        {
            LanguageChanged?.Invoke(newLocale.Identifier.Code);
        }
    }
}