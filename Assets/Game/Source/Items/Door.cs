using DG.Tweening;
using Game.Source.Base;
using Game.Source.Character;
using Game.Source.Items.Base;
using Game.Source.Models;
using UnityEngine;

namespace Game.Source.Items
{
    [RequireComponent(typeof(Collider))]
    public class Door : InteractiveItem, ITarget
    {
        [Range(0f, 30f), SerializeField] private float _interactDelay = 0f;
        [SerializeField] private Transform _door;
        [SerializeField] private bool _isOpenStart = false;
        [SerializeField] private Vector3 _openAngle = new(0, 90, 0);
        [Range(0.01f, 50f), SerializeField] private float _openTime = 5f;

        [Min(1), SerializeField] private int _weight = 5;

        private bool _isOpen;

        private Vector3 _startRotation;
        private Collider _collider;

        private void Awake()
        {
            _startRotation = _door.rotation.eulerAngles;
            _collider = GetComponent<Collider>();

            ChangeState(_isOpenStart, true);
        }

        public override float InteractDelay => _interactDelay;

        public override bool CanInteract(object interactor)
        {
            return true;
        }

        public override void Interact(object interactor)
        {
            if (interactor is Player _)
                ChangeState(_isOpen == false);
        }

        private Tweener _tween;

        private void ChangeState(bool isOpen, bool isForce = false)
        {
            _isOpen = isOpen;

            _collider.enabled = false;

            _tween?.Kill();

            _tween = _door.DORotate(isOpen ? _startRotation + _openAngle : _startRotation, _openTime)
                .OnComplete(() => _collider.enabled = true);

            if (isForce)
                _tween.Complete(true);
        }

        public Vector3 Position => transform.position;

        public int Weight => _isOpen ? 0 : _weight;

        public Team Team => Team.None;
    }
}