namespace DoubleDCore.Community
{
    public interface ILeaderboardService
    {
        public void Open(Leaderboard leaderboard, Order order = Order.DESC, int depth = 10, int neighborsDepth = 0);

        public Leaderboard GetLeaderboard(string id, string region = null);
    }
}