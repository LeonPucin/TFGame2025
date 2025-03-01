namespace DoubleDCore.Effects.Base
{
    public interface IStatusEffectListener<in TStatusEffect> where TStatusEffect : IStatusEffect
    {
        public void OnStatusAdded(TStatusEffect statusEffect);

        public void OnStatusUpdated(TStatusEffect newStatusEffect);

        public void OnStatusRemoved(TStatusEffect statusEffect);
    }
}