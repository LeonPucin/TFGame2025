using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace DoubleDCore.Donation.Base
{
    public interface IDonationService
    {
        public bool IsDonationAvailable { get; }

        public bool IsSubscriptionAvailable { get; }

        public UniTask<IEnumerable<DonationOrder>> GetOrders();

        public UniTask<IEnumerable<DonationPurchase>> GetPurchases();

        public UniTask<string> Donate(string donationId);

        public UniTask<string> ConsumeDonation(string donationId);

        public UniTask<string> Subscribe(string subscribeId);

        public UniTask<string> Unsubscribe(string subscribeId);
    }
}