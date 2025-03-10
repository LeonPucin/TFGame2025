using UnityEngine;
using UnityEngine.Localization;

namespace Game.Source.Items.Base
{
    public abstract class SelectableItem : MonoBehaviour, ISelectableObject
    {
        [SerializeField] private LocalizedString _name;

        public LocalizedString Name => _name;

        public abstract void Select();

        public abstract void Deselect();
    }
}