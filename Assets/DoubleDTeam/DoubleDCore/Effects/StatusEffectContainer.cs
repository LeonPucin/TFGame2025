using System;
using System.Collections.Generic;
using System.Linq;
using DoubleDCore.Effects.Base;
using DoubleDCore.EventActions;
using DoubleDCore.OS.Compression;
using DoubleDCore.Storage.Base;
using UnityEngine;
using Zenject;

namespace DoubleDCore.Effects
{
    public class StatusEffectContainer : IStatusEffectContainer
    {
        private readonly Dictionary<Type, IStatusEffect> _statusEffects = new();

        public event Action<IStatusEffect> StatusEffectAdded;
        public event Action<IStatusEffect> StatusEffectUpdated;
        public event Action<IStatusEffect> StatusEffectRemoved;

        public readonly EventBus StatusEffectAddedBus = new();
        public readonly EventBus StatusEffectUpdatedBus = new();
        public readonly EventBus StatusEffectRemovedBus = new();

        public IEnumerable<IStatusEffect> StatusEffects => _statusEffects.Values;

        public string Key { get; }

        private readonly DiContainer _diContainer;

        public StatusEffectContainer(string saveKey, DiContainer diContainer)
        {
            Key = saveKey;
            _diContainer = diContainer;
        }

        public bool Has<TStatusEffect>() where TStatusEffect : IStatusEffect
        {
            return _statusEffects.ContainsKey(typeof(TStatusEffect));
        }

        private bool Has(IStatusEffect statusEffect)
        {
            return _statusEffects.ContainsKey(statusEffect.GetType());
        }

        public void Add<TStatusEffect>(TStatusEffect statusEffect) where TStatusEffect : class, IStatusEffect
        {
            Type statusEffectType = statusEffect.GetType();

            if (Has<TStatusEffect>())
            {
                var currentStatusEffect = Get<TStatusEffect>();

                if (currentStatusEffect is IStackableStatusEffect<TStatusEffect> stackableEffect)
                    stackableEffect.Stack(statusEffect);
                else
                    _statusEffects[statusEffectType] = statusEffect;

                var updatedStatusEffect = Get<TStatusEffect>();

                StatusEffectUpdatedBus.Invoke(updatedStatusEffect);
                StatusEffectUpdated?.Invoke(updatedStatusEffect);

                return;
            }

            _statusEffects.Add(statusEffectType, statusEffect);

            StatusEffectAddedBus.Invoke(statusEffect);
            StatusEffectAdded?.Invoke(statusEffect);
        }

        private void DynamicAdd(IStatusEffect statusEffect)
        {
            Type type = statusEffect.GetType();

            if (typeof(IStatusEffect).IsAssignableFrom(type) == false)
            {
                Debug.LogError($"Type {type.Name} is not a valid StatusEffect type");
                return;
            }

            var method = typeof(StatusEffectContainer).GetMethod(nameof(Add))?.MakeGenericMethod(type);

            if (method == null)
            {
                Debug.LogError($"Method {nameof(Add)} not found");
                return;
            }

            method.Invoke(this, new object[] { statusEffect });
        }

        public TStatusEffect Get<TStatusEffect>() where TStatusEffect : class, IStatusEffect
        {
            var type = typeof(TStatusEffect);

            if (Has<TStatusEffect>())
                return _statusEffects[type] as TStatusEffect;

            Debug.LogError($"Status effect of type {type.Name} not found");

            return default;
        }

        public void Remove<TStatusEffect>() where TStatusEffect : class, IStatusEffect
        {
            if (Has<TStatusEffect>() == false)
                return;

            Remove(Get<TStatusEffect>());
        }

        private void Remove(IStatusEffect statusEffect)
        {
            if (Has(statusEffect) == false)
                return;

            Type statusEffectType = statusEffect.GetType();

            _statusEffects.Remove(statusEffectType);

            StatusEffectRemovedBus.Invoke(statusEffect);
            StatusEffectRemoved?.Invoke(statusEffect);
        }

        public void Clear()
        {
            foreach (var statusEffect in StatusEffects)
                Remove(statusEffect);

            _statusEffects.Clear();
        }

        string ISaveObject.GetData()
        {
            var data = StatusEffects.Select(s => new EncryptStatusEffect
            {
                TypeName = s.GetType().AssemblyQualifiedName,
                Data = s.GetData()
            }).ToArray();

            return CompressionService.CompressBase64(new EncryptStatusEffectContainer
            {
                StatusEffects = data
            });
        }

        string ISaveObject.GetDefaultData()
        {
            return CompressionService.CompressBase64(new EncryptStatusEffectContainer());
        }

        void ISaveObject.OnLoad(string data)
        {
            var encrypt = CompressionService.DecompressBase64<EncryptStatusEffectContainer>(data);

            Clear();

            foreach (var statusEffectData in encrypt.StatusEffects)
            {
                var statusEffect = Create(statusEffectData.TypeName);
                statusEffect.OnLoad(statusEffectData.Data);
                DynamicAdd(statusEffect);
            }
        }

        private IStatusEffect Create(string typeName)
        {
            Type type = Type.GetType(typeName);

            if (type == null)
                throw new ArgumentException($"StatusEffect with type '{typeName}' not found.");

            return _diContainer.Instantiate(type) as IStatusEffect;
        }

        [Serializable]
        private class EncryptStatusEffectContainer
        {
            public EncryptStatusEffect[] StatusEffects = Array.Empty<EncryptStatusEffect>();
        }

        [Serializable]
        private class EncryptStatusEffect
        {
            public string TypeName;
            public string Data;
        }
    }
}