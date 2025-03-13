using UnityEngine;
using UnityEngine.Localization;

namespace Game.Source.Items.Base
{
    public abstract class SelectableItem : MonoBehaviour, ISelectableObject
    {
        [SerializeField] private LocalizedString _name;

        public LocalizedString Name => _name;

        public abstract void Select(object selector);

        public abstract void Deselect(object selector);
    }
}