using DoubleDCore.Effects.Base;
using UnityEngine;
using Zenject;

namespace DoubleDCore.Effects
{
    public abstract class MonoStatusEffectListener<TStatusEffect> : MonoBehaviour,
        IStatusEffectListener<TStatusEffect> where TStatusEffect : class, IStatusEffect
    {
        protected IStatusEffectContainer StatusEffectContainer;

        [Inject]
        private void Init(IStatusEffectContainer statusEffectContainer)
        {
            StatusEffectContainer = statusEffectContainer;
        }

        private void OnEnable()
        {
            StatusEffectContainer.StatusEffectAdded += OnStatusAddedHandler;
            StatusEffectContainer.StatusEffectUpdated += OnStatusUpdatedHandler;
            StatusEffectContainer.StatusEffectRemoved += OnStatusRemovedHandler;
        }

        private void OnDisable()
        {
            StatusEffectContainer.StatusEffectAdded -= OnStatusAddedHandler;
            StatusEffectContainer.StatusEffectUpdated -= OnStatusUpdatedHandler;
            StatusEffectContainer.StatusEffectRemoved -= OnStatusRemovedHandler;
        }

        private void OnStatusAddedHandler(IStatusEffect statusEffect)
        {
            if (statusEffect is TStatusEffect effect)
                OnStatusAdded(effect);
        }

        private void OnStatusUpdatedHandler(IStatusEffect statusEffect)
        {
            if (statusEffect is TStatusEffect effect)
                OnStatusUpdated(effect);
        }

        private void OnStatusRemovedHandler(IStatusEffect statusEffect)
        {
            if (statusEffect is TStatusEffect effect)
                OnStatusRemoved(effect);
        }

        public abstract void OnStatusAdded(TStatusEffect statusEffect);
        public abstract void OnStatusUpdated(TStatusEffect newStatusEffect);
        public abstract void OnStatusRemoved(TStatusEffect statusEffect);
    }
}