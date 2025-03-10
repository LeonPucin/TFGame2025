using System;

namespace Game.Source.Items.Base
{
    public interface ITakeableObject : ISelectableObject
    {
        public TakeableType TakeableType { get; }

        public void Take();

        public void Drop();
    }

    [Flags]
    public enum TakeableType
    {
        None = 0,
        OneHanded = 1 << 0,
        TwoHanded = 1 << 1,
        Everything = OneHanded | TwoHanded
    }
}