﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using DoubleDCore.Extensions;
using DoubleDCore.Storage.Base;
using UnityEngine;

namespace DoubleDCore.Storage
{
    public class FileSaver : ISaveController
    {
        private readonly string _saveFilePath = Application.persistentDataPath + "/save.json";

        private readonly SaveFile _saveFile;

        private readonly Dictionary<string, ISaveObject> _saveObjects = new();

        public FileSaver()
        {
            string data;

            if (File.Exists(_saveFilePath) == false)
            {
                var stream = File.CreateText(_saveFilePath);
                stream.Close();
            }

            using (var reader = new StreamReader(_saveFilePath))
                data = reader.ReadToEnd();

            _saveFile = string.IsNullOrEmpty(data)
                ? new SaveFile()
                : SaveFile.Deserialize(data);

            if (_saveFile.IsValidFile == false)
                Debug.LogWarning("File damaged");
        }

        public void Subscribe(ISaveObject saveObject)
        {
            if (_saveObjects.TryAdd(saveObject.Key, saveObject))
                return;

            _saveObjects[saveObject.Key] = saveObject;
        }

        public void Unsubscribe(string key)
        {
            _saveObjects.Remove(key);
        }

        public void Save(string key)
        {
            _Save(key, true);
        }

        public void SaveAll()
        {
            foreach (var (key, _) in _saveObjects)
                _Save(key, false);

            PushSave();
        }

        public void Load(string key)
        {
            if (ContainSaveObject(key) == false)
            {
                Debug.LogError($"Key {key} does not register");
                return;
            }

            ISaveObject saveObject = _saveObjects[key];
            bool hasParameter = _saveFile.ContainsParameter(key);

            if (hasParameter)
            {
                var saveParameter = _saveFile.GetSaveParameterInfo(key);

                saveObject.OnLoad(saveParameter.Data);

                if (saveParameter.IsValidParameter == false)
                    Debug.LogWarning($"Parameter {saveParameter.Key} damaged");
            }
            else
            {
                saveObject.OnLoad(saveObject.GetDefaultData());
            }
        }

        public void LoadAll()
        {
            foreach (var (key, _) in _saveObjects.ToArray())
                Load(key);
        }

        public void Reset(string key)
        {
            if (_saveObjects.TryGetValue(key, out var saveObject) == false)
                return;

            if (_saveFile.ContainsParameter(key) == false)
                return;

            _saveFile.UpdateParameter(key, saveObject.GetDefaultData());
        }

        public void ResetAll()
        {
            foreach (var (key, _) in _saveObjects)
                Reset(key);
        }

        public bool ContainSaveObject(string key)
        {
            return _saveObjects.ContainsKey(key);
        }

        public void Clear()
        {
            File.Delete(_saveFilePath);
        }

        private void _Save(string key, bool isPushed)
        {
            if (ContainSaveObject(key) == false)
            {
                Debug.LogError($"Key {key} does not register");
                return;
            }

            string objectData = _saveObjects[key].GetData();

            bool hasParameter = _saveFile.ContainsParameter(key);

            if (hasParameter == false)
            {
                _saveFile.AddParameter(new SaveParameter(key, objectData));

                if (isPushed)
                    PushSave();

                return;
            }

            var saveParameter = _saveFile.GetSaveParameterInfo(key);

            if (saveParameter.Hash == SaveParameter.GetHash(objectData))
                return;

            _saveFile.UpdateParameter(key, objectData);

            if (isPushed)
                PushSave();
        }

        private void PushSave()
        {
            string data = _saveFile.Serialize();

            using (var streamWriter = new StreamWriter(_saveFilePath, false))
                streamWriter.Write(data);

            Debug.Log((nameof(FileSaver) + " save").Color(Color.green));
        }
    }
}