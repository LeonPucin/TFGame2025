namespace DoubleDCore.Storage.Base
{
    public interface ISaveController
    {
        public void Subscribe(ISaveObject saveObject);

        public void Unsubscribe(string key);

        public void Save(string key);

        public void SaveAll();

        public void Load(string key);

        public void LoadAll();

        public void Reset(string key);

        public void ResetAll();

        public bool ContainSaveObject(string key);

        public void Clear();
    }
}