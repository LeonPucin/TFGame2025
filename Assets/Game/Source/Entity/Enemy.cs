using DoubleDCore.Debugging;
using Game.Source.Entity.Base;
using UnityEngine;

namespace Game.Source.Entity
{
    public class Enemy : DamageableTarget
    {
        [Min(1), SerializeField] private float _maxHealth = 100f;

        private Health _health;

        protected override void Awake()
        {
            _health = new Health(_maxHealth);

            base.Awake();
        }

        protected override void OnEnable()
        {
            _health.Died += OnKill;
            _health.HealthChanged += OnHealthChanged;

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            _health.Died -= OnKill;
            _health.HealthChanged -= OnHealthChanged;

            base.OnDisable();
        }

        public override void TakeDamage(float damage)
        {
            _health.ApplyDamage(damage);
        }

        public override void Kill()
        {
            _health.Kill();
        }

        private void OnHealthChanged(float newValue)
        {
            ScreenDebug.Log(GetInstanceID(),
                $"Health changed: {newValue}", transform.position, color: Color.red, 10f);
        }

        private void OnKill()
        {
            Destroy(gameObject);
        }
    }
}