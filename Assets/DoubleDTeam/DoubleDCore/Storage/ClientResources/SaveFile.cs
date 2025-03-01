using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DoubleDCore.Extensions;
using UnityEngine;

namespace DoubleDCore.Storage
{
    [Serializable]
    public class SaveFile : ISerializationCallbackReceiver
    {
        [SerializeField] private string _createDate;
        [SerializeField] private string _changeDate;
        [SerializeField] private string _serializeDate;
        [SerializeField] private SaveParameter[] _data;
        [SerializeField] private string _hash;

        private readonly Dictionary<string, SaveParameter> _dataDictionary = new();

        public bool IsValidFile { get; private set; }

        public string CreateDate => _createDate;
        public string ChangeDate => _changeDate;
        public string SerializeDate => _serializeDate;

        public IEnumerable<SaveParameter> Date => _dataDictionary.Values;

        public SaveFile()
        {
            IsValidFile = true;
            _createDate = GetDate();
            _changeDate = _createDate;
        }

        public void AddParameter(SaveParameter parameter)
        {
            bool isSucceed = _dataDictionary.TryAdd(parameter.Key, parameter);

            if (isSucceed)
                UpdateChangeDate();
        }

        public void UpdateParameter(string key, string data)
        {
            if (TryGetParameter(key, out var saveParameter) == false)
                return;

            saveParameter.UpdateParameter(data);

            UpdateChangeDate();
        }

        public void RemoveParameter(string key)
        {
            bool isSucceed = _dataDictionary.Remove(key);

            if (isSucceed)
                UpdateChangeDate();
        }

        public bool ContainsParameter(string key)
        {
            return _dataDictionary.ContainsKey(key);
        }

        private bool TryGetParameter(string key, out SaveParameter saveParameter)
        {
            saveParameter = null;

            if (ContainsParameter(key) == false)
                return false;

            saveParameter = _dataDictionary[key];

            return true;
        }

        public SaveParameterInfo GetSaveParameterInfo(string key)
        {
            if (TryGetParameter(key, out var saveParameter))
                return saveParameter.CreateSaveParameterInfo();

            Debug.LogError($"Key {key} does not exist");
            return null;
        }

        public string Serialize()
        {
            _serializeDate = GetDate();
            return JsonUtility.ToJson(this);
        }

        public static SaveFile Deserialize(string saveFileData)
            => JsonUtility.FromJson<SaveFile>(saveFileData);

        public void OnBeforeSerialize()
        {
            _data = _dataDictionary.Values.ToArray();

            RecalculateHash(_data);
        }

        public void OnAfterDeserialize()
        {
            foreach (var saveParameter in _data)
                AddParameter(saveParameter);

            IsValidFile = _hash == GetHash(_data);
        }

        private string GetHash(IEnumerable<SaveParameter> data)
        {
            var concatData = string.Concat(data.Select(d => d.Data));

            return concatData.GetCRC32();
        }

        public void RecalculateHash(IEnumerable<SaveParameter> data)
        {
            _hash = GetHash(data);
        }

        private string GetDate()
            => DateTime.Now.ToString("G", DateTimeFormatInfo.InvariantInfo);

        private void UpdateChangeDate()
        {
            _changeDate = GetDate();
        }
    }
}