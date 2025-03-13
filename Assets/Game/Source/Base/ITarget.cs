using Game.Source.Models;
using UnityEngine;

namespace Game.Source.Base
{
    public interface ITarget
    {
        public Vector3 Position { get; }
        public int Weight { get; }
        public Team Team { get; }
    }
}