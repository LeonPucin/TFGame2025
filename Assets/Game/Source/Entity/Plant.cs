using System.Collections.Generic;
using Game.Source.Items;
using Game.Source.Items.Base;
using Game.Source.Storage;
using UnityEngine;

namespace Game.Source.Entity
{
    public class Plant : InteractiveItem
    {
        [SerializeField] private float _takeDelay;
        [Min(0), SerializeField] private float _maturationTime;
        [SerializeField] private List<Produce> _products;

        private Queue<Produce> _produceQueue;

        public float MaturationProgress => _currentMaturationTime / _maturationTime;

        public bool IsGrown => MaturationProgress >= 1f;
        public bool IsHarvested => ProductCount == 0;

        public int ProductCount => _produceQueue.Count;

        private float _currentMaturationTime;

        public void AddGrowingTime(float delta)
        {
            _currentMaturationTime += delta;

            if (IsGrown)
                GrowUp();
        }

        private void Start()
        {
            foreach (var product in _products)
            {
                product.SetKinematic(true);
                product.gameObject.SetActive(false);
            }

            _produceQueue = new Queue<Produce>(_products);
        }

        private void GrowUp()
        {
            foreach (var product in _products)
                product.gameObject.SetActive(true);
        }

        public override float InteractDelay => _takeDelay;

        public override bool CanInteract(object interactor)
        {
            return IsGrown && IsHarvested == false && interactor is IReceiver<TakeableItem>;
        }

        public override void Interact(object interactor)
        {
            if (interactor is not IReceiver<TakeableItem> receiver)
                return;

            var produce = _produceQueue.Dequeue();
            produce.SetKinematic(false);

            if (receiver.Peek() is ProduceTray { IsFull: false } tray)
            {
                tray.AddContent(produce);
                return;
            }

            receiver.Put(produce);
        }
    }
}