using System;
using UnityEngine;

namespace DoubleDCore.Donation.Base
{
    public record DonationOrder(
        int ID,
        string Name,
        string Description,
        Texture Icon,
        int Price,
        string Currency,
        bool IsSubscription,
        TimeSpan Period,
        TimeSpan TrialPeriod)
    {
        public int ID { get; } = ID;
        public string Name { get; } = Name;
        public string Description { get; } = Description;
        public Texture Icon { get; } = Icon;
        public int Price { get; } = Price;
        public string Currency { get; } = Currency;
        public bool IsSubscription { get; } = IsSubscription;
        public TimeSpan Period { get; } = Period;
        public TimeSpan TrialPeriod { get; } = TrialPeriod;
    }
}