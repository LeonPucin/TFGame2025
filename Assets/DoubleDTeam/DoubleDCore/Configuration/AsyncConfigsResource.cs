using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DoubleDCore.Configuration.Base;
using DoubleDCore.Storage.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DoubleDCore.Configuration
{
    public class AsyncConfigsResource : IConfigsResource, IAsyncResource
    {
        private const string ConfigKey = "Configs";

        private readonly Dictionary<Type, ScriptableConfig> _configs = new();

        public TConfigType Get<TConfigType>() where TConfigType : ScriptableConfig
        {
            var type = typeof(TConfigType);

            if (_configs.TryGetValue(type, out var config))
                return config as TConfigType;

            Debug.LogError($"Config {type.Name} not found");
            return null;
        }

        private void BindConfig(ScriptableConfig scriptableObject)
        {
            var type = scriptableObject.GetType();

            _configs.TryAdd(type, scriptableObject);
        }

        public async UniTask Load()
        {
            var handle = Addressables.LoadResourceLocationsAsync(ConfigKey);

            await handle.Task;

            if (handle.Status is AsyncOperationStatus.Failed or AsyncOperationStatus.None)
                return;

            foreach (var location in handle.Result)
            {
                var operationHandle = Addressables.LoadAssetAsync<ScriptableConfig>(location);

                await operationHandle.Task;

                if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                    BindConfig(operationHandle.Result);
            }

            Addressables.Release(handle);
        }

        public void Release()
        {
            foreach (var (_, config) in _configs)
                Addressables.Release(config);

            _configs.Clear();
        }
    }
}