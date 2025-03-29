namespace DoubleDCore.PhysicsTools.Casting
{
    public interface ICaster<in TDataType, out TColliderType> where TDataType : BaseCastInfo
    {
        public bool IsActive { get; }

        public void AddListener<TTarget>(ITargetListener<TTarget, TColliderType> listener, TDataType castInfo);

        public void RemoveListener<TTarget>(ITargetListener<TTarget, TColliderType> listener);

        public void StartCast();

        public void StopCast();
    }
}