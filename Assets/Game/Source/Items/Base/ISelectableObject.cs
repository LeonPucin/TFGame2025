using UnityEngine.Localization;

namespace Game.Source.Items.Base
{
    public interface ISelectableObject
    {
        public LocalizedString Name { get; }

        public void Select();

        public void Deselect();
    }
}