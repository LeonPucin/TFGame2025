using Game.Source.Configs;

namespace Game.Source.Models
{
    public class WorldRule
    {
        public readonly TeamMatrix TeamMatrix;
        public readonly WorldRuleConfig Config;

        public WorldRule(TeamMatrix teamMatrix, WorldRuleConfig config)
        {
            TeamMatrix = teamMatrix;
            Config = config;
        }
    }
}