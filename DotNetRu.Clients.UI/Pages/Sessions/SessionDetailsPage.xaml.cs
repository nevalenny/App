﻿namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.DataObjects;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.DataObjects;

    public partial class SessionDetailsPage
    {
        private IPlatformSpecificExtension<Session> _extension;

        public override AppPage PageType => AppPage.Session;

        SessionDetailsViewModel ViewModel => vm ?? (vm = BindingContext as SessionDetailsViewModel);

        SessionDetailsViewModel vm;

        public SessionDetailsPage(Session session)
        {
            InitializeComponent();

            _extension = DependencyService.Get<IPlatformSpecificExtension<Session>>();

            ItemId = session?.Title;

            ListViewSpeakers.ItemSelected += async (sender, e) =>
                {
                    var speaker = ListViewSpeakers.SelectedItem as Speaker;
                    if (speaker == null) return;

                    ContentPage destination;

                    if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                    {
                        var speakerDetailsUwp = new SpeakerDetailsPageUWP(vm.Session.Id);
                        speakerDetailsUwp.Speaker = speaker;
                        destination = speakerDetailsUwp;
                    }
                    else
                    {
                        var speakerDetails = new SpeakerDetailsPage(vm.Session.Id);
                        speakerDetails.Speaker = speaker;
                        destination = speakerDetails;
                    }

                    await NavigationService.PushAsync(Navigation, destination);
                    ListViewSpeakers.SelectedItem = null;
                };


            ButtonRate.Clicked += async (sender, e) =>
                {
                    await NavigationService.PushModalAsync(
                        Navigation,
                        new EvolveNavigationPage(new FeedbackPage(ViewModel.Session)));
                };
            BindingContext = new SessionDetailsViewModel(Navigation, session);
            ViewModel.LoadSessionCommand.Execute(null);

        }

        void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null) return;
            list.SelectedItem = null;
        }

        void MainScroll_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (e.ScrollY > SessionDate.Y) Title = ViewModel.Session.ShortTitle;
            else Title = "Session Details";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MainScroll.Scrolled += MainScroll_Scrolled;
            ListViewSpeakers.ItemTapped += ListViewTapped;


            var count = ViewModel?.Session?.Speakers?.Count ?? 0;
            var adjust = Device.OS != TargetPlatform.Android ? 1 : -count + 1;
            if ((ViewModel?.Session?.Speakers?.Count ?? 0) > 0)
                ListViewSpeakers.HeightRequest = (count * ListViewSpeakers.RowHeight) - adjust;

            if (_extension != null)
            {
                await _extension.Execute(ViewModel.Session);
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            MainScroll.Scrolled -= MainScroll_Scrolled;
            ListViewSpeakers.ItemTapped -= ListViewTapped;

            if (_extension != null)
            {
                await _extension.Finish();
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = null;

            ListViewSpeakers.HeightRequest = 0;

            var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.SessionMaterialItems.Count + 2;
            ListViewSessionMaterial.HeightRequest =
                (ViewModel.SessionMaterialItems.Count * ListViewSessionMaterial.RowHeight) - adjust;
        }
    }
}

