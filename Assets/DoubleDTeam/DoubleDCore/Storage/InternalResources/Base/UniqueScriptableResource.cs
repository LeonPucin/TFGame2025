using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDCore.Storage.Base
{
    public abstract class UniqueScriptableResource<TScriptableType> : IResource where TScriptableType : ScriptableObject
    {
        protected abstract string ResourcePath { get; }

        private readonly Dictionary<Type, TScriptableType> _objects = new();

        public bool Contains<TObject>() where TObject : TScriptableType
        {
            return _objects.ContainsKey(typeof(TObject));
        }

        public TObject Get<TObject>() where TObject : TScriptableType
        {
            var type = typeof(TObject);

            if (Contains<TObject>())
                return _objects[type] as TObject;

            Debug.LogError($"Resource of type {type.Name} not found");
            return default;
        }

        public void Load()
        {
            var items = Resources.LoadAll<TScriptableType>(ResourcePath);

            foreach (var item in items)
                _objects.TryAdd(item.GetType(), item);
        }

        public void Release()
        {
            _objects.Clear();
        }
    }
}