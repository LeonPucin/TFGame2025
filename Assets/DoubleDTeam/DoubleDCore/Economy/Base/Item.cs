using DoubleDCore.Identification;
using UnityEngine;
using UnityEngine.Localization;

namespace DoubleDCore.Economy.Base
{
    public abstract class Item : IdentifyingScriptable
    {
        [SerializeField] private LocalizedString _name;
        [SerializeField] private LocalizedString _description;
        [SerializeField] private Sprite _sprite;

        public LocalizedString Name => _name;
        public LocalizedString Description => _description;
        public Sprite Sprite => _sprite;

        protected abstract override string GetIDPrefix();
    }
}