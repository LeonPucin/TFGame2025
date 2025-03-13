using Cinemachine;
using Cysharp.Threading.Tasks;
using Game.Source.Extensions;
using Game.Source.Items.Base;
using Game.Source.Storage;
using UnityEngine;

namespace Game.Source.Character
{
    [RequireComponent(typeof(Player), typeof(CharacterController))]
    public class CharacterReceiver : MonoBehaviour
    {
        [SerializeField] private Transform _rightHandItemContainer;
        [SerializeField] private Transform _bothHandsItemContainer;

        [Space, SerializeField] private CinemachineVirtualCamera _camera;
        [Range(0f, 100f), SerializeField] private float _dropForce = 10f;
        [SerializeField] private Transform _dropContainer;

        private Receiver<TakeableItem> _receiver;
        private CharacterController _characterController;

        private void Awake()
        {
            _receiver = GetComponent<Player>().Receiver;
            _characterController = GetComponent<CharacterController>();
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

            obj.SetKinematic(true);

            obj.Take();
        }

        private async void ReceiverOnTake(TakeableItem obj)
        {
            obj.transform.parent = _dropContainer;
            obj.transform.parent = null;

            obj.SetKinematic(false);

            await UniTask.WaitForFixedUpdate();

            obj.Rigidbody.MovePosition(_dropContainer.position);
            obj.Rigidbody.MoveRotation(_dropContainer.rotation);

            if (_characterController.velocity.magnitude > 0.1f)
                obj.Rigidbody.AddForce(_characterController.velocity, ForceMode.VelocityChange);

            obj.Rigidbody.AddForce(_camera.GetForward() * _dropForce, ForceMode.Impulse);

            obj.Drop();
        }
    }
}