using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DoubleDCore.Donation.Base;
using DoubleDCore.Extensions;
using UnityEngine;

namespace DoubleDCore.Donation
{
    public class MockDonationService : IDonationService
    {
        public bool IsDonationAvailable => true;
        public bool IsSubscriptionAvailable => true;

        public UniTask<IEnumerable<DonationOrder>> GetOrders()
        {
            var tcs = new UniTaskCompletionSource<IEnumerable<DonationOrder>>();

            var orders = new List<DonationOrder>
            {
                new(
                    1,
                    "MOCK_ORDER_1",
                    "Mock order description",
                    null,
                    100,
                    "RUB",
                    false,
                    TimeSpan.FromDays(0),
                    TimeSpan.FromDays(0)
                ),
                new(
                    2,
                    "MOCK_ORDER_2",
                    "Mock order description",
                    null,
                    299,
                    "RUB",
                    true,
                    TimeSpan.FromDays(1),
                    TimeSpan.FromDays(0)
                )
            };

            tcs.TrySetResult(orders);
            return tcs.Task;
        }

        public UniTask<IEnumerable<DonationPurchase>> GetPurchases()
        {
            var tcs = new UniTaskCompletionSource<IEnumerable<DonationPurchase>>();

            var purchases = new List<DonationPurchase>
            {
                new(
                    2,
                    DateTime.Now,
                    false,
                    true,
                    DateTime.Now.Add(TimeSpan.FromDays(3))
                ),
            };

            tcs.TrySetResult(purchases);
            return tcs.Task;
        }

        public UniTask<string> Donate(string donationId)
        {
            var tcs = new UniTaskCompletionSource<string>();
            tcs.TrySetResult(donationId);
            Debug.Log($"Donation with id {donationId} has been successfully donated".Color(Color.green));
            return tcs.Task;
        }

        public UniTask<string> ConsumeDonation(string donationId)
        {
            var tcs = new UniTaskCompletionSource<string>();
            tcs.TrySetResult(donationId);
            Debug.Log($"Donation with id {donationId} has been successfully consumed".Color(Color.green));
            return tcs.Task;
        }

        public UniTask<string> Subscribe(string subscribeId)
        {
            var tcs = new UniTaskCompletionSource<string>();
            tcs.TrySetResult(subscribeId);
            Debug.Log($"Subscription with id {subscribeId} has been successfully subscribed".Color(Color.green));
            return tcs.Task;
        }

        public UniTask<string> Unsubscribe(string subscribeId)
        {
            var tcs = new UniTaskCompletionSource<string>();
            tcs.TrySetResult(subscribeId);
            Debug.Log($"Subscription with id {subscribeId} has been successfully unsubscribed".Color(Color.green));
            return tcs.Task;
        }
    }
}