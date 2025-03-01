using System;
using System.Collections.Generic;
using DoubleDCore.Periphery.Base;
using UnityEngine;

namespace Infrastructure.Input
{
    public class InputService : IInputServices<InputLevers>
    {
        private readonly InputLevers _inputControls;

        private readonly Dictionary<Type, Map> _maps = new();

        public InputService(InputLevers inputControls)
        {
            _inputControls = inputControls;
        }

        public InputLevers GetInputProvider()
        {
            return _inputControls;
        }

        public void SwitchMap<TMap>() where TMap : Map
        {
            var targetMap = GetMap<TMap>();

            foreach (var map in _maps.Values)
            {
                if (map.IsActive)
                {
                    map.Disable();
                    Debug.Log($"Map {map.GetType().Name} disabled");
                }
            }

            targetMap.Enable();
            Debug.Log($"Map {targetMap.GetType().Name} enabled");
        }

        public void Enable()
        {
            _inputControls.Enable();
        }

        public void Disable()
        {
            _inputControls.Disable();
        }

        private Map GetMap<TMap>() where TMap : Map
        {
            var type = typeof(TMap);

            if (_maps.TryGetValue(type, out var map) == false)
                Debug.LogError($"Map of type {type} not found");

            return map;
        }

        public void AddMap(Map map)
        {
            var type = map.GetType();

            if (_maps.TryAdd(type, map) == false)
                Debug.LogError($"Map of type {type} already exists");
        }
    }
}