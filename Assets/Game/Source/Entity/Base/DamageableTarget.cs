using Game.Source.AI;
using Game.Source.Base;
using Game.Source.Models;
using UnityEngine;

namespace Game.Source.Entity.Base
{
    public abstract class DamageableTarget : AIBrain, IDamageable, ITarget
    {
        [SerializeField] private Team _team;
        [Min(1), SerializeField] private int _weightNeuron;

        public Vector3 Position => transform.position;
        public Team Team => _team;
        public int Weight => _weightNeuron;

        public abstract void TakeDamage(float damage);
        public abstract void Kill();
    }
}