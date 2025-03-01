namespace DoubleDCore.Automation.Base
{
    public interface IState : IExitableState
    {
        public void Enter();
    }
}