using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace DoubleDCore.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class TranslatedText : MonoBehaviour
    {
        [SerializeField] private LocalizedString _stringKey;

        private TMP_Text _text;
        private TextLocalization _localizationText;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();

            _localizationText = new TextLocalization(_stringKey, translatedText => _text.text = translatedText);
        }

        public void InsertParameters(params object[] parameters)
        {
            if (_localizationText == null)
            {
                Debug.LogError("Localization text is not initialized!");
                return;
            }

            _localizationText.InsertParameters(parameters);
        }
    }
}