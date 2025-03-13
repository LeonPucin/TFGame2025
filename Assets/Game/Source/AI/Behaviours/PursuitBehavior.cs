using Game.Source.Base;

namespace Game.Source.AI.Behaviours
{
    public class PursuitBehavior : NodeBehavior
    {
        private ITarget _currentTarget;

        public void StartPursuit(ITarget currentTarget)
        {
            _currentTarget = currentTarget;

            Agent.isStopped = false;

            Agent.SetDestination(_currentTarget.Position);
        }

        public void StopPursuit()
        {
            _currentTarget = null;

            Agent.isStopped = true;
        }
    }
}