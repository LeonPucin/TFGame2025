using Cinemachine;
using UnityEngine;

namespace Game.Source.Character
{
    public class CameraContainer : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _syncCamera;
        [Range(0.01f, 10f), SerializeField] private float _smoothSpeed = 1f;

        private Vector3 _velocity = Vector3.zero;

        private void Update()
        {
            if (_syncCamera == null)
            {
                Debug.LogWarning("Sync camera is not set");
                return;
            }

            Vector3 currentPosition = transform.position;
            Quaternion currentRotation = transform.rotation;

            Vector3 camPosition = _syncCamera.State.FinalPosition;
            Quaternion camRotation = _syncCamera.State.FinalOrientation;

            transform.position =
                Vector3.SmoothDamp(currentPosition, camPosition, ref _velocity, _smoothSpeed);

            transform.rotation =
                Quaternion.Lerp(currentRotation, camRotation, _smoothSpeed);
        }
    }
}