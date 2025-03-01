using System;

namespace DoubleDCore.Donation.Base
{
    public record DonationPurchase(
        int ID,
        DateTime CreateDate,
        bool IsGift,
        bool Subscribed,
        DateTime ExpiryDate
    )
    {
        public int ID { get; } = ID;
        public DateTime CreateDate { get; } = CreateDate;
        public bool IsGift { get; } = IsGift;
        public bool Subscribed { get; } = Subscribed;
        public DateTime ExpiryDate { get; } = ExpiryDate;
    }
}