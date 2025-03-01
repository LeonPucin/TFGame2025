using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DoubleDCore.Extensions;
using UnityEngine;

namespace DoubleDCore.Community
{
    public class MockLeaderboardService : ILeaderboardService
    {
        public void Open(Leaderboard leaderboard, Order order = Order.DESC, int depth = 10, int neighborsDepth = 0)
        {
            var players = new List<LeaderboardMember>();

            players.AddRange(leaderboard.TopPlayers);

            Debug.Log($"Leaderboard {leaderboard.ID} openned".Color(Color.green));
            Debug.Log("Leaderboard: " + leaderboard.ID + '\n' +
                      string.Join('\n', players.ConvertAll(player => player.Name + ' ' + player.Value)));
        }

        public Leaderboard GetLeaderboard(string id, string region = null)
        {
            return new MockLeaderboard(id, region);
        }
    }

    public class MockLeaderboard : Leaderboard
    {
        protected override List<LeaderboardMember> InternalTopPlayers { get; set; }

        public MockLeaderboard(string id, string region) : base(id, region)
        {
        }

        public override UniTask Fetch(Order order = Order.DESC, int depth = 10, int neighborsDepth = 0)
        {
            InternalTopPlayers = new List<LeaderboardMember>
            {
                new(1080, "Top1", "", 1, "999"),
                new(1070, "Top2", "", 2, "990"),
                new(1060, "Top3", "", 3, "800"),
                new(1050, "Top4", "", 4, "750"),
                new(1040, "Top5", "", 5, "749"),
            };

            Debug.Log($"Leaderboard {ID} fetched".Color(Color.yellow));

            return UniTask.CompletedTask;
        }

        public override UniTask PublishRecord(string key, float value)
        {
            Debug.Log($"Record {key} published with value {value}".Color(Color.green));
            return UniTask.CompletedTask;
        }
    }
}