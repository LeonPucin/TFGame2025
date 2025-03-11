using DoubleDCore.Localization.Base;
using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using Game.Source.Items.Base;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace Game.Source.UI.Pages
{
    public class ActionTipPage : MonoPage, IPayloadPage<ActionItem>
    {
        [SerializeField] private TMP_Text _actionTipText;

        private LocalizedString _description;

        private ILocalizationService _localizationService;

        [Inject]
        private void Init(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public override void Initialize()
        {
            SetCanvasState(false);
        }

        public void Open(ActionItem context)
        {
            _description = context.ActionDescription;
            SetText();

            _localizationService.LanguageChanged += OnLanguageChanged;

            SetCanvasState(true);
        }

        public override void Close()
        {
            SetCanvasState(false);

            _localizationService.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(string obj)
        {
            SetText();
        }

        private async void SetText()
        {
            _actionTipText.text = await _localizationService.GetTranslation(_description.TableEntryReference);
        }
    }
}