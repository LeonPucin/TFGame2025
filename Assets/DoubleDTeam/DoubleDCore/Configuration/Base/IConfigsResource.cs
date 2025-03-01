namespace DoubleDCore.Configuration.Base
{
    public interface IConfigsResource
    {
        public TConfigType Get<TConfigType>() where TConfigType : ScriptableConfig;
    }
}