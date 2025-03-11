using Cinemachine;
using Game.Source.Extensions;
using Game.Source.Items.Base;
using Game.Source.Storage;
using UnityEngine;

namespace Game.Source.Character
{
    [RequireComponent(typeof(Player))]
    public class CharacterReceiver : MonoBehaviour
    {
        [SerializeField] private Transform _rightHandItemContainer;
        [SerializeField] private Transform _bothHandsItemContainer;

        [Space, SerializeField] private CinemachineVirtualCamera _camera;
        [Range(0f, 100f), SerializeField] private float _dropForce = 10f;
        [SerializeField] private Transform _dropContainer;

        private Receiver<TakeableItem> _receiver;

        private void Awake()
        {
            _receiver = GetComponent<Player>().Receiver;
        }

        private void OnEnable()
        {
            _receiver.OnPut += ReceiverOnPut;
            _receiver.OnTake += ReceiverOnTake;
        }

        private void OnDisable()
        {
            _receiver.OnPut -= ReceiverOnPut;
            _receiver.OnTake -= ReceiverOnTake;
        }

        private void ReceiverOnPut(TakeableItem obj)
        {
            if (obj.TakeableType.HasFlag(TakeableType.TwoHanded))
                obj.transform.parent = _bothHandsItemContainer;

            if (obj.TakeableType.HasFlag(TakeableType.OneHanded))
                obj.transform.parent = _rightHandItemContainer;

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            obj.Rigidbody.isKinematic = true;
            obj.Rigidbody.interpolation = RigidbodyInterpolation.None;
            obj.Collider.enabled = false;

            obj.Take();
        }

        private void ReceiverOnTake(TakeableItem obj)
        {
            obj.transform.parent = _dropContainer;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            obj.transform.parent = null;

            obj.Rigidbody.isKinematic = false;
            obj.Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            obj.Rigidbody.AddForce(_camera.GetForward() * _dropForce, ForceMode.Impulse);

            obj.Collider.enabled = true;

            obj.Drop();
        }
    }
}