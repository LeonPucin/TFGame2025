namespace DoubleDCore.PhysicsTools.Casting
{
    public interface ITargetListener<TTarget, in TColliderType>
    {
        public TTarget GetTarget(TColliderType target);

        public bool IsTarget(TTarget target);

        public void OnCastEnter(TTarget target);

        public void OnCastExit(TTarget target);
    }
}