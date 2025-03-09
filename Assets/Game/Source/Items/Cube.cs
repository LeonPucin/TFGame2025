using Game.Source.Interactive;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Source.Items
{
    public class Cube : MonoBehaviour, IInteractiveObject
    {
        [SerializeField] private LocalizedString _name;
        [SerializeField] private float _interactDelay;

        private InteractiveInfo _interactiveInfo;

        public InteractiveInfo InteractiveInfo => _interactiveInfo;

        private void Awake()
        {
            _interactiveInfo = new InteractiveInfo(_name, _interactDelay);
        }

        public void Interact()
        {
            transform.position += Vector3.up;
        }
    }
}