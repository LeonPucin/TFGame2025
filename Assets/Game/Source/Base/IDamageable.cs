namespace Game.Source.Base
{
    public interface IDamageable
    {
        public void TakeDamage(float damage);

        public void Kill();
    }
}