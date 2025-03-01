using DoubleDCore.Storage.Base;
using Zenject;

namespace DoubleDCore.Localization.Base
{
    public class LocaleSave : ISaveObject
    {
        private readonly ILocalizationService _localizationService;

        [Inject]
        public LocaleSave(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public string Key => "Language";

        public string GetData()
        {
            return _localizationService.GetLanguage();
        }

        public string GetDefaultData()
        {
            return "en";
        }

        public void OnLoad(string data)
        {
            _localizationService.SetLanguage(data);
        }
    }
}