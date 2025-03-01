namespace DoubleDCore.Automation.Base
{
    public interface IPayloadedState<in TPayload> : IExitableState
    {
        public void Enter(TPayload payload);
    }
}