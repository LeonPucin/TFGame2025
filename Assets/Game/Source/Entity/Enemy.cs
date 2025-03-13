using DoubleDCore.Debugging;
using DoubleDCore.Fabrics.Base;
using Game.Source.Entity.Base;
using Game.Source.Items;
using Game.Source.Models;
using UnityEngine;
using Zenject;

namespace Game.Source.Entity
{
    public class Enemy : DamageableTarget
    {
        [Min(1), SerializeField] private float _maxHealth = 100f;
        [SerializeField] private Produce _loot;
        [Min(1), SerializeField] private int _lootAmount = 4;

        private Health _health;

        private IPrefabFabric _prefabFabric;

        [Inject]
        private void Init(IPrefabFabric prefabFabric)
        {
            _prefabFabric = prefabFabric;
        }

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

            SpawnLoot();
        }

        private void SpawnLoot()
        {
            for (int i = 0; i < _lootAmount; i++)
            {
                var loot = _prefabFabric.Create(_loot, transform.position, Quaternion.identity, null);
                loot.SetKinematic(false);
            }
        }
    }
}