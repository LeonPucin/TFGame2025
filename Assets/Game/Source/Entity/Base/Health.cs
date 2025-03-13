using System;
using UnityEngine;

namespace Game.Source.Entity.Base
{
    public class Health
    {
        private float _health;
        private readonly float _maxHealth;

        private bool _isDead;

        public event Action<float> HealthChanged;
        public event Action Died;

        public float Amount
        {
            get => _health;
            private set
            {
                if (_isDead)
                    return;

                _health = Mathf.Clamp(value, 0, _maxHealth);

                HealthChanged?.Invoke(_health);

                if (_health <= 0)
                {
                    _isDead = true;
                    Died?.Invoke();
                }
            }
        }

        public Health(float maxHealth)
        {
            _maxHealth = maxHealth;
            _health = _maxHealth;
        }

        public void ApplyDamage(float damage)
        {
            Amount -= damage;
        }

        public void ApplyHealing(float healing)
        {
            Amount += healing;
        }

        public void Kill()
        {
            ApplyDamage(Amount);
        }
    }
}