using DoubleDCore.Economy.Base;
using Game.Source.Entity.Base;
using UnityEngine;

namespace Game.Source.Entity
{
    public class BuyButton : EntityButton
    {
        [Min(0), SerializeField] private int _cost;
        [SerializeField] private AudioClip _clickSound;
        public int Cost => _cost;

        public override bool CanInteract(object interactor)
        {
            if (interactor is not IWallet<int> wallet)
                return false;

            return wallet.Value >= _cost;
        }

        public override void Interact(object interactor)
        {
            AudioSource.PlayClipAtPoint(_clickSound, transform.position);
        }
    }
}