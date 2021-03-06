﻿using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;

using DotNetRu.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using XamarinEvolve.Clients.Portable;

[assembly: ExportRenderer(typeof(XamarinEvolve.Clients.UI.NavigationView), typeof(NavigationViewRenderer))]

namespace DotNetRu.Droid
{
    using XamarinEvolve.Utils.Helpers;

    public class NavigationViewRenderer : ViewRenderer<XamarinEvolve.Clients.UI.NavigationView, NavigationView>
    {
        NavigationView navView;

        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);

            navView.Menu.FindItem(Resource.Id.nav_feed).SetTitle($"{EventInfo.EventName}");
            navView.Menu.FindItem(Resource.Id.nav_events).SetVisible(true);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<XamarinEvolve.Clients.UI.NavigationView> e)
        {

            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null) return;


            var view = Inflate(Forms.Context, Resource.Layout.nav_view, null);
            navView = view.JavaCast<NavigationView>();


            navView.NavigationItemSelected += NavView_NavigationItemSelected;

            Settings.Current.PropertyChanged += SettingsPropertyChanged;
            SetNativeControl(navView);

            navView.SetCheckedItem(Resource.Id.nav_feed);
        }

        void SettingsPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        public override void OnViewRemoved(Android.Views.View child)
        {
            base.OnViewRemoved(child);
            navView.NavigationItemSelected -= NavView_NavigationItemSelected;
            Settings.Current.PropertyChanged -= SettingsPropertyChanged;
        }

        IMenuItem previousItem;

        void NavView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {


            if (previousItem != null) previousItem.SetChecked(false);

            navView.SetCheckedItem(e.MenuItem.ItemId);

            previousItem = e.MenuItem;

            int id = 0;
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.nav_feed:
                    id = (int)AppPage.Feed;
                    break;
                case Resource.Id.nav_speakers:
                    id = (int)AppPage.Speakers;
                    break;
                case Resource.Id.nav_events:
                    id = (int)AppPage.Events;
                    break;
                case Resource.Id.nav_sponsors:
                    id = (int)AppPage.Sponsors;
                    break;
                case Resource.Id.nav_settings:
                    id = (int)AppPage.Settings;
                    break;
            }
            this.Element.OnNavigationItemSelected(
                new XamarinEvolve.Clients.UI.NavigationItemSelectedEventArgs { Index = id });
        }
    }
}

