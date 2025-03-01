namespace DoubleDCore.Storage.Base
{
    public interface ISaveObject
    {
        public string Key { get; }

        public string GetData();

        public string GetDefaultData();

        public void OnLoad(string data);
    }
}