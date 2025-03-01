using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace DoubleDCore.Community
{
    public abstract class Leaderboard
    {
        public readonly string ID;
        public readonly string Region;

        protected Leaderboard(string id, string region)
        {
            ID = id;
            Region = region;
        }

        public IReadOnlyList<LeaderboardMember> TopPlayers => InternalTopPlayers;

        protected abstract List<LeaderboardMember> InternalTopPlayers { get; set; }

        public abstract UniTask Fetch(Order order = Order.DESC, int depth = 10, int neighborsDepth = 0);

        public abstract UniTask PublishRecord(string key, float value);
    }
}