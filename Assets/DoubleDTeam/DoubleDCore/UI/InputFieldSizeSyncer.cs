using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubleDCore.UI
{
    [RequireComponent(typeof(LayoutElement)), RequireComponent(typeof(TMP_InputField))]
    public class InputFieldSizeSyncer : MonoBehaviour
    {
        private TMP_InputField _inputField;
        private LayoutElement _layoutElement;

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
            _layoutElement = GetComponent<LayoutElement>();
        }

        private void Update()
        {
            _layoutElement.preferredWidth = _inputField.preferredWidth;
            _layoutElement.preferredHeight = _inputField.preferredHeight;
        }
    }
}