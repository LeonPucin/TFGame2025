using Cysharp.Threading.Tasks;
using DoubleDCore.EventActions;
using DoubleDCore.Localization.Base;
using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using Game.Source.Interactive;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Source.UI.Pages
{
    public class InteractiveObjectPage : MonoPage, IPayloadPage<InteractiveObjectPageArgument>
    {
        [SerializeField] private TMP_Text _interactiveObjectName;
        [SerializeField] private Image _pointer;

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
                await _localizationService.GetTranslation(_context.InteractiveInfo.Name.TableEntryReference);
        }

        private void OnInteractProgress(float newValue)
        {
            _pointer.fillAmount = 1 - newValue;
        }
    }

    public class InteractiveObjectPageArgument
    {
        public readonly InteractiveInfo InteractiveInfo;
        public readonly ActionReference<float> InteractProgress;

        public InteractiveObjectPageArgument(InteractiveInfo interactiveInfo, ActionReference<float> interactProgress)
        {
            InteractiveInfo = interactiveInfo;
            InteractProgress = interactProgress;
        }
    }
}