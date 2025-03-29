using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoubleDCore.PhysicsTools.Casting.Raycasting
{
    public class RayCaster : IRayCaster<Collider>
    {
        private bool _isActive;

        private readonly List<TargetListenerInfoBase> _targetsInfo = new();

        public bool IsActive => _isActive;

        private RayCaster()
        {
        }

        public void AddListener<TTarget>(ITargetListener<TTarget, Collider> listener, RayCastInfo castInfo)
        {
            if (_targetsInfo.Any(t => t.Listener.Equals(listener)))
            {
                Debug.LogError($"Ray listener {listener.GetType().Name} already exists");
                return;
            }

            _targetsInfo.Add(new TargetListenerInfo<TTarget>(listener, castInfo));
        }

        public void RemoveListener<TTarget>(ITargetListener<TTarget, Collider> listener)
        {
            var info = _targetsInfo.FirstOrDefault(t => t.Listener.Equals(listener));

            if (info != null)
                _targetsInfo.Remove(info);
        }

        public void StartCast()
        {
            _isActive = true;
        }

        public void StopCast()
        {
            _isActive = false;
        }

        public void FixedTick()
        {
            if (!_isActive)
                return;

            foreach (var listenerInfo in _targetsInfo)
                ListenerHandler(listenerInfo);
        }

        private void ListenerHandler(TargetListenerInfoBase listenerInfo)
        {
            bool hasHitInfo = Physics.Raycast(
                listenerInfo.RayInfo.Ray,
                out RaycastHit hitInfo,
                listenerInfo.RayInfo.RayMaxDistance,
                listenerInfo.RayInfo.Mask,
                listenerInfo.RayInfo.QueryTriggerInteraction);

            object target = null;
            bool isTarget = false;

            if (hasHitInfo)
            {
                target = listenerInfo.GetTarget(hitInfo.collider);
                isTarget = listenerInfo.IsTarget(target);
            }

            if (hasHitInfo == false || target == null || isTarget == false)
            {
                if (listenerInfo.LastTarget == null)
                    return;

                listenerInfo.OnCastExit(listenerInfo.LastTarget);
                listenerInfo.LastTarget = null;

                return;
            }

            if (listenerInfo.LastTarget != null && listenerInfo.LastTarget.Equals(target) == false)
            {
                listenerInfo.OnCastExit(listenerInfo.LastTarget);
                listenerInfo.LastTarget = null;
            }

            if (listenerInfo.LastTarget != null)
                return;

            listenerInfo.LastTarget = target;
            listenerInfo.OnCastEnter(target);
        }

        public void Dispose()
        {
            _targetsInfo.Clear();
        }

        private abstract class TargetListenerInfoBase
        {
            public readonly RayCastInfo RayInfo;

            protected TargetListenerInfoBase(RayCastInfo rayInfo)
            {
                RayInfo = rayInfo;
            }

            public abstract object Listener { get; }

            public abstract object LastTarget { get; set; }

            public abstract object GetTarget(Collider target);

            public abstract bool IsTarget(object target);

            public abstract void OnCastEnter(object target);

            public abstract void OnCastExit(object target);
        }

        private class TargetListenerInfo<TTarget> : TargetListenerInfoBase
        {
            private readonly ITargetListener<TTarget, Collider> _listener;
            private TTarget _lastTarget;

            public override object Listener => _listener;

            public override object LastTarget
            {
                get => _lastTarget;
                set => _lastTarget = (TTarget)value;
            }

            public TargetListenerInfo(ITargetListener<TTarget, Collider> listener, RayCastInfo rayInfo)
                : base(rayInfo)
            {
                _listener = listener;
            }

            public override object GetTarget(Collider target)
            {
                return _listener.GetTarget(target);
            }

            public override bool IsTarget(object target)
            {
                return _listener.IsTarget((TTarget)target);
            }

            public override void OnCastEnter(object target)
            {
                _listener.OnCastEnter((TTarget)target);
            }

            public override void OnCastExit(object target)
            {
                _listener.OnCastExit((TTarget)target);
            }
        }
    }
}