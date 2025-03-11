using UnityEngine.Localization;

namespace Game.Source.Items.Base
{
    public interface IActionObject
    {
        public LocalizedString ActionDescription { get; }

        public void Action(object actor);
    }
}