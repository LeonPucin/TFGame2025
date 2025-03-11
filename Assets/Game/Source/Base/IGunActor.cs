using UnityEngine;

namespace Game.Source.Base
{
    public interface IGunActor
    {
        public Ray ShootRay { get; }
    }
}