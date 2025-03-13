namespace Infrastructure
{
    public class BootstrapInfo
    {
        public readonly string MainMenuSceneName;
        public readonly string GameloopSceneName;

        public BootstrapInfo(string mainMenuSceneName, string gameloopSceneName)
        {
            MainMenuSceneName = mainMenuSceneName;
            GameloopSceneName = gameloopSceneName;
        }
    }
}