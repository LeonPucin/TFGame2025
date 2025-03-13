using DoubleDCore.Configuration.Base;
using UnityEngine;

namespace Game.Source.Configs
{
    [CreateAssetMenu(fileName = "WorldRuleConfig", menuName = "Configs/WorldRuleConfig")]
    public class WorldRuleConfig : ScriptableConfig
    {
        [SerializeField] private bool _friendlyFire;
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private LayerMask _shootMask;

        public bool FriendlyFire => _friendlyFire;
        public LayerMask TargetMask => _targetMask;
        public LayerMask ShootMask => _shootMask;
    }
}