namespace DoubleDCore.Storage
{
    public class SaveParameterInfo
    {
        public string Key { get; }
        public string Data { get; }
        public string Hash { get; }
        public bool IsValidParameter { get; }

        public SaveParameterInfo(string key, string data, string hash, bool isValidParameter)
        {
            Key = key;
            Data = data;
            Hash = hash;
            IsValidParameter = isValidParameter;
        }
    }
}