using DoubleDCore.Debugging;
using Game.Source.Base;
using UnityEngine;

namespace Game.Source.Items
{
    public class ShootTarget : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float damage)
        {
            ScreenDebug.Log(777, $"Damage taken: {damage}", transform.position, color: Color.red, 10f);
        }

        public void Kill()
        {
            ScreenDebug.Log(777, "Target killed", transform.position, color: Color.red, 10f);
        }
    }
}