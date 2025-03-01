using System;
using System.Collections.Generic;
using System.Linq;
using DoubleDCore.Pooling.Base;
using UnityEngine;

namespace DoubleDCore.Pooling
{
    public class Pooler<TPoolingType> : IPooler<TPoolingType>
    {
        private readonly Queue<PoolerBox> _queue = new();
        private readonly Dictionary<TPoolingType, PoolerBox> _borrowed = new();

        public int Count => _queue.Count;

        public Pooler()
        {
        }

        public Pooler(TPoolingType poolingObject, Action<TPoolingType> onGet = null,
            Action<TPoolingType> onReturn = null)
        {
            Push(poolingObject, onGet, onReturn);
        }

        public Pooler(IEnumerable<TPoolingType> poolingObjects, Action<TPoolingType> onGet = null,
            Action<TPoolingType> onReturn = null)
        {
            foreach (var poolingObject in poolingObjects)
                Push(poolingObject, onGet, onReturn);
        }

        public void Push(TPoolingType poolingObject, Action<TPoolingType> onGet = null,
            Action<TPoolingType> onReturn = null)
        {
            _queue.Enqueue(new PoolerBox
            {
                PoolingObject = poolingObject,
                OnGet = onGet,
                OnReturn = onReturn
            });
        }

        public TPoolingType Get()
        {
            bool isSuccess = TryGet(out var result);

            if (isSuccess == false)
                throw new Exception("Pool is empty");

            return result;
        }

        public bool TryGet(out TPoolingType result)
        {
            result = default;

            if (Count <= 0)
                return false;

            var box = _queue.Dequeue();
            _borrowed.Add(box.PoolingObject, box);

            result = box.PoolingObject;

            box.OnGet?.Invoke(result);

            return true;
        }

        public void Return(TPoolingType poolingObject)
        {
            if (_borrowed.Remove(poolingObject, out var box) == false)
            {
                Debug.LogError($"Attempt to return an object {typeof(TPoolingType)} that is not borrowed");
                return;
            }

            _queue.Enqueue(box);

            box.OnReturn?.Invoke(poolingObject);
        }

        public void Clear()
        {
            foreach (var poolingObject in _borrowed.Keys.ToArray())
                Return(poolingObject);

            _queue.Clear();
            _borrowed.Clear();
        }

        private class PoolerBox
        {
            public TPoolingType PoolingObject;
            public Action<TPoolingType> OnGet;
            public Action<TPoolingType> OnReturn;
        }
    }
}