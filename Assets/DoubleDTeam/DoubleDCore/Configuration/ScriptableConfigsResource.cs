using DoubleDCore.Configuration.Base;
using DoubleDCore.Storage.Base;

namespace DoubleDCore.Configuration
{
    public class ScriptableConfigsResource : UniqueScriptableResource<ScriptableConfig>, IConfigsResource
    {
        protected override string ResourcePath => "Configs";
    }
}