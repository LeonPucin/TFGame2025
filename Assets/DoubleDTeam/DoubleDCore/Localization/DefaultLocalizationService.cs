using DoubleDCore.Localization.Base;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace DoubleDCore.Localization
{
    public class DefaultLocalizationService : BaseLocalizationService
    {
        public override void SetAutoLanguage()
        {
            if (SetLanguage(Application.systemLanguage.ToString()) == false)
                SetDefaultLanguage();
        }

        public override void SetDefaultLanguage()
        {
            SetLanguage(LocalizationSettings.ProjectLocale.Identifier.Code);
        }
    }
}