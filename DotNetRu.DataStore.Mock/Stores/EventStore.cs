﻿namespace XamarinEvolve.DataStore.Mock.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.DataStore.Mock.Abstractions;

    public class EventStore : BaseStore<FeaturedEvent>, IEventStore
    {
        List<FeaturedEvent> Events { get; }
        ISponsorStore _sponsors;
        public EventStore()
        {
            this.Events = new List<FeaturedEvent>();
            this._sponsors = DependencyService.Get<ISponsorStore>();
        }

        public override async Task InitializeStore()
        {
            if (this.Events.Count != 0)
                return;

            var sponsorList = await this._sponsors.GetItemsAsync();
                        
            this.Events.Add(new FeaturedEvent
                {
                    Title = "Evening Event",
                    Description = "",
                    StartTime = new DateTime(2016, 4, 26, 23, 0, 0, DateTimeKind.Utc),
                    EndTime = new DateTime(2016, 4, 27, 1, 0, 0, DateTimeKind.Utc),
                LocationName = string.Empty,
                    IsAllDay = false,
                });

            this.Events.Add(new FeaturedEvent
                {
                    Title = "Happy Hour",
                    Description = "",
                    StartTime = new DateTime(2016, 4, 27, 22, 30, 0, DateTimeKind.Utc),
                    EndTime = new DateTime(2016, 4, 28, 0, 0, 0, DateTimeKind.Utc),
                    LocationName = "Expo Hall",
                    IsAllDay = false,
                    Sponsor = sponsorList.FirstOrDefault(x => x.Name == "Microsoft")
                });

            this.Events.Add(new FeaturedEvent
                {
                    Title = "General Session",
                    Description = "",
                    StartTime = new DateTime(2016, 4, 28, 13, 0, 0, DateTimeKind.Utc),
                    EndTime = new DateTime(2016, 4, 28, 14, 30, 0, DateTimeKind.Utc),
                    LocationName ="General Session",
                    IsAllDay = false,
                });
        }

        public override async Task<IEnumerable<FeaturedEvent>> GetItemsAsync(bool forceRefresh = false)
        {
            await this.InitializeStore();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject (this.Events);
            return this.Events;
        }
    }
}

