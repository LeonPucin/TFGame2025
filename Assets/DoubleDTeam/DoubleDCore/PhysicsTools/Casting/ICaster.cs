namespace DoubleDCore.PhysicsTools.Casting
{
    public interface ICaster<in TDataType> where TDataType : BaseCastInfo
    {
        public bool IsActive { get; }

        public void AddListener<TTarget>(ITargetListener<TTarget> listener, TDataType castInfo);

        public void RemoveListener<TTarget>(ITargetListener<TTarget> listener);

        public void StartCast();

        public void StopCast();
    }
}