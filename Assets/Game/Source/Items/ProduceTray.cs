using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DoubleDCore.Identification;
using Game.Source.Items.Base;
using Game.Source.Storage;
using UnityEngine;

namespace Game.Source.Items
{
    public class ProduceTray : TakeableItem, IIdentifying
    {
        [Range(0f, 30f), SerializeField] private float _putProduceDelay = 0.5f;
        [SerializeField] private Vector3Int _grid = new(3, 1, 2);
        [SerializeField] private BoxCollider _fillArea;

        private readonly List<Produce> _content = new();

        private bool _isProduceSelected;

        public string ID => Name.TableEntryReference.KeyId.ToString();

        public int Capacity => _grid.x * _grid.y * _grid.z;

        public bool IsFull => _content.Count >= Capacity;

        public IReadOnlyList<Produce> Content => _content;

        public override float InteractDelay => _isProduceSelected ? _putProduceDelay : TakeDelay;


        public override void Interact(object interactor)
        {
            if (interactor is IReceiver<TakeableItem> receiver && receiver.Peek() is Produce produce)
            {
                if (IsFull == false)
                {
                    receiver.Take();
                    AddContent(produce);
                    return;
                }
            }

            base.Interact(interactor);
        }

        public override void Select(object selector)
        {
            _isProduceSelected = selector is IReceiver<TakeableItem> receiver && receiver.Peek() is Produce;
        }

        public override void Deselect(object selector)
        {
            _isProduceSelected = false;
        }

        public async void AddContent(Produce produce)
        {
            _content.Add(produce);

            _isProduceSelected = false;

            await UniTask.WaitForFixedUpdate();

            Rigidbody.mass += produce.Mass;

            produce.transform.SetParent(_fillArea.transform);
            produce.transform.localPosition = GetLocalPositionForProduce();
            produce.transform.localRotation = Quaternion.identity;

            produce.SetKinematic(true);
        }

        private Vector3 GetLocalPositionForProduce()
        {
            int index = _content.Count - 1;
            int cols = Mathf.RoundToInt(_grid.x);
            int floors = Mathf.RoundToInt(_grid.y);
            int rows = Mathf.RoundToInt(_grid.z);

            int colIndex = index % cols;
            int rowIndex = (index / cols) % rows;
            int floorIndex = index / (cols * rows);

            Vector3 size = _fillArea.size;

            float cellWidth = size.x / cols;
            float cellHeight = size.y / floors;
            float cellDepth = size.z / rows;

            float x = -size.x / 2f + cellWidth * (colIndex + 0.5f);
            float y = -size.y / 2f + cellHeight * (floorIndex + 0.5f);
            float z = -size.z / 2f + cellDepth * (rowIndex + 0.5f);

            return new Vector3(x, y, z) + _fillArea.center;
        }
    }
}