using Cysharp.Threading.Tasks;
using Game.Source.Entity;
using Game.Source.Models;
using UnityEngine;
using Zenject;

namespace Game.Source
{
    public class EnemyChecker : MonoBehaviour
    {
        [Min(0), SerializeField] private float _checkRate = 1f;
        [Min(0), SerializeField] private float _checkRadius = 70f;

        private GasManager _gasManager;
        private LayerMask _targetLayer;

        [Inject]
        private void Init(GasManager gasManager, WorldRule worldRule)
        {
            _gasManager = gasManager;
            _targetLayer = worldRule.Config.TargetMask;
        }

        private void OnEnable()
        {
            _gasManager.GasEvent += OnGasEvent;
        }

        private void OnDisable()
        {
            _gasManager.GasEvent -= OnGasEvent;
        }

        private readonly Collider[] _buffer = new Collider[128];

        private async void OnGasEvent()
        {
            int enemyCount;

            do
            {
                await UniTask.WaitForSeconds(_checkRate);

                enemyCount = 0;

                int count = Physics.OverlapSphereNonAlloc(transform.position, _checkRadius, _buffer, _targetLayer);

                for (int i = 0; i < count; i++)
                {
                    var target = _buffer[i];

                    if (target.TryGetComponent(out Enemy _))
                        enemyCount++;
                }
            } while (enemyCount > 0);

            _gasManager.StopGas();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _checkRadius);
        }
    }
}