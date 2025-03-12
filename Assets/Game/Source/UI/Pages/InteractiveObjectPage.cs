using Cysharp.Threading.Tasks;
using DoubleDCore.EventActions;
using DoubleDCore.Localization.Base;
using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Zenject;

namespace Game.Source.UI.Pages
{
    public class InteractiveObjectPage : MonoPage, IPayloadPage<InteractiveObjectPageArgument>
    {
        [SerializeField] private TMP_Text _interactiveObjectName;
        [SerializeField] private Image _selectPointer;
        [SerializeField] private Image _interactPointer;

        private InteractiveObjectPageArgument _context;

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

        public async void Open(InteractiveObjectPageArgument context)
        {
            _context = context;

            _localizationService.LanguageChanged += OnLanguageChanged;
            await SetText();

            _context.InteractProgress.AddListener(OnInteractProgress);
            OnInteractProgress(0);

            _selectPointer.enabled = _context.CanInteract == false;
            _interactPointer.enabled = _context.CanInteract;

            SetCanvasState(true);
        }

        public override void Close()
        {
            _localizationService.LanguageChanged -= OnLanguageChanged;

            _context.InteractProgress.RemoveListener(OnInteractProgress);

            SetCanvasState(false);
        }

        private void OnLanguageChanged(string obj)
        {
            _ = SetText();
        }

        private async UniTask SetText()
        {
            _interactiveObjectName.text =
                await _localizationService.GetTranslation(_context.Name.TableEntryReference);
        }

        private void OnInteractProgress(float newValue)
        {
            _interactPointer.fillAmount = 1 - newValue;
        }
    }

    public class InteractiveObjectPageArgument
    {
        public readonly LocalizedString Name;
        public readonly bool CanInteract;
        public readonly ActionReference<float> InteractProgress;

        public InteractiveObjectPageArgument(LocalizedString name, bool canInteract,
            ActionReference<float> interactProgress)
        {
            Name = name;
            CanInteract = canInteract;
            InteractProgress = interactProgress;
        }
    }
}