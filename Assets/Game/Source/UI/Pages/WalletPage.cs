using DoubleDCore.Economy.Base;
using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using TMPro;
using UnityEngine;

namespace Game.Source.UI.Pages
{
    public class WalletPage : MonoPage, IPayloadPage<IWallet<int>>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _format = "${0}";

        private IWallet<int> _context;

        public override void Initialize()
        {
            SetCanvasState(false);
        }

        public void Open(IWallet<int> context)
        {
            _context = context;

            _context.ValueChanged += OnValueChanged;
            OnValueChanged(_context.Value, 0);

            SetCanvasState(true);
        }

        public override void Close()
        {
            _context.ValueChanged -= OnValueChanged;

            SetCanvasState(false);
        }

        private void OnValueChanged(int currentValue, int delta)
        {
            _text.text = string.Format(_format, currentValue);
        }
    }
}