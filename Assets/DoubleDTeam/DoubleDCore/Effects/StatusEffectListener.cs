using System;
using DoubleDCore.Effects.Base;

namespace DoubleDCore.Effects
{
    public abstract class StatusEffectListener<TStatusEffect> : IStatusEffectListener<TStatusEffect>, IDisposable
        where TStatusEffect : class, IStatusEffect
    {
        protected readonly IStatusEffectContainer StatusEffectContainer;

        protected StatusEffectListener(IStatusEffectContainer statusEffectContainer)
        {
            StatusEffectContainer = statusEffectContainer;

            StatusEffectContainer.StatusEffectAdded += OnStatusAddedHandler;
            StatusEffectContainer.StatusEffectUpdated += OnStatusUpdatedHandler;
            StatusEffectContainer.StatusEffectRemoved += OnStatusRemovedHandler;
        }

        public void Dispose()
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