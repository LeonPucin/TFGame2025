using UnityEngine.Localization;

namespace Game.Source.Interactive
{
    public class InteractiveInfo
    {
        public LocalizedString Name { get; }
        public float InteractDelay { get; }

        public InteractiveInfo(LocalizedString name, float interactDelay = 0)
        {
            Name = name;
            InteractDelay = interactDelay;
        }
    }
}