using UnityEngine.Localization;

namespace Game.Source.Items.Base
{
    public interface ISelectableObject
    {
        public LocalizedString Name { get; }

        public void Select(object selector);

        public void Deselect(object selector);
    }
}