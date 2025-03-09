using Cinemachine;
using UnityEngine;

namespace Game.Source.Extensions
{
    public static class CinemachineExtensions
    {
        public static Vector3 GetForward(this CinemachineVirtualCamera camera)
        {
            return camera.State.FinalOrientation * Vector3.forward;
        }

        public static Vector3 GetRight(this CinemachineVirtualCamera camera)
        {
            return camera.State.FinalOrientation * Vector3.right;
        }
    }
}