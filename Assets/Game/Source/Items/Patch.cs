using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Source.Entity;
using Game.Source.Items.Base;
using Game.Source.Storage;
using UnityEngine;

namespace Game.Source.Items
{
    public class Patch : InteractiveItem
    {
        [Range(0f, 30f), SerializeField] private float _interactDelay = 0.5f;
        [SerializeField] private BoxCollider _fillArea;
        [SerializeField] private Vector2Int _fillGrid = new(5, 2);

        public override float InteractDelay => _interactDelay;

        private readonly List<PlantInfo> _content = new();

        public int Capacity => Grid.x * Grid.y * Grid.z;

        private Vector3Int Grid => new(_fillGrid.x, 1, _fillGrid.y);

        public override bool CanInteract(object interactor)
        {
            if (_content.Count >= Capacity)
                return false;

            if (interactor is not IReceiver<TakeableItem> receiver)
                return false;

            if (receiver.Peek() is not SeedsBag seedsBag)
                return false;

            return seedsBag.Seeds > 0;
        }

        public override void Interact(object interactor)
        {
            if (_content.Count >= Capacity)
                return;

            if (interactor is not IReceiver<TakeableItem> receiver)
                return;

            if (receiver.Peek() is not SeedsBag seedsBag)
                return;

            seedsBag.Seeds--;

            var plant = seedsBag.GetPlant();

            var info = new PlantInfo()
            {
                Plant = plant,
                Index = FindFreeIndex()
            };

            _content.Add(info);

            plant.transform.position = GetLocalPositionForProduce(info.Index);
            plant.transform.rotation = Quaternion.identity;
            plant.transform.SetParent(transform);
        }

        private void Start()
        {
            StartGrowing();
        }

        private bool _isGrowing;

        public void StartGrowing()
        {
            if (_isGrowing)
                return;

            _isGrowing = true;
            GrowingCycle();
        }

        public void StopGrowing()
        {
            _isGrowing = false;
        }

        public List<int> _blackPlantsIndexes = new();

        private async void GrowingCycle()
        {
            while (_isGrowing)
            {
                await UniTask.WaitForFixedUpdate();

                foreach (var plantInfo in _content)
                {
                    if (plantInfo.Plant.IsGrown)
                    {
                        if (plantInfo.Plant.IsHarvested)
                        {
                            _blackPlantsIndexes.Add(plantInfo.Index);
                        }

                        continue;
                    }

                    plantInfo.Plant.AddGrowingTime(Time.fixedDeltaTime);
                }

                _blackPlantsIndexes.ForEach(index =>
                {
                    var listIndex = _content.FindIndex(x => x.Index == index);
                    var harvestedPlant = _content[listIndex];

                    _content.RemoveAt(listIndex);

                    Destroy(harvestedPlant.Plant.gameObject);
                });

                _blackPlantsIndexes.Clear();
            }
        }

        private int FindFreeIndex()
        {
            var locked = _content.Select(x => x.Index).ToArray();

            for (int i = 0; i < Capacity; i++)
            {
                if (locked.Contains(i) == false)
                    return i;
            }

            return -1;
        }

        private Vector3 GetLocalPositionForProduce(int index)
        {
            int cols = Mathf.RoundToInt(Grid.x);
            int floors = Mathf.RoundToInt(Grid.y);
            int rows = Mathf.RoundToInt(Grid.z);

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

        private class PlantInfo
        {
            public Plant Plant;
            public int Index;
        }
    }
}