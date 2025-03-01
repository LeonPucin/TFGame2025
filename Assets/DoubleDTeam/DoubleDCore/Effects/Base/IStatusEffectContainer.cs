using System;
using System.Collections.Generic;
using DoubleDCore.Storage.Base;

namespace DoubleDCore.Effects.Base
{
    public interface IStatusEffectContainer : ISaveObject
    {
        event Action<IStatusEffect> StatusEffectAdded;
        event Action<IStatusEffect> StatusEffectUpdated;
        event Action<IStatusEffect> StatusEffectRemoved;

        IEnumerable<IStatusEffect> StatusEffects { get; }

        public bool Has<TStatusEffect>() where TStatusEffect : IStatusEffect;
        void Add<TStatusEffect>(TStatusEffect statusEffect) where TStatusEffect : class, IStatusEffect;
        public TStatusEffect Get<TStatusEffect>() where TStatusEffect : class, IStatusEffect;
        public void Remove<TStatusEffect>() where TStatusEffect : class, IStatusEffect;

        public void Clear();
    }
}