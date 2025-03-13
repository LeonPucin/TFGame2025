using UnityEngine;
using UnityEngine.Localization;

namespace Game.Source.Items.Base
{
    public abstract class ActionItem : TakeableItem, IActionObject
    {
        [SerializeField] private LocalizedString _actionDescription;

        public LocalizedString ActionDescription => _actionDescription;

        public abstract void StartAction(object actor);

        public abstract void StopAction(object actor);
    }
}