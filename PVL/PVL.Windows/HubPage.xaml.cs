// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955
using System;
using System.Collections.ObjectModel;
using Windows.Media;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PVL.Common;
using PVL.Data;
using PVL.DataModel;

namespace PVL
{
    /// <summary>
    ///     A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly NavigationHelper navigationHelper;
        private readonly SystemMediaTransportControls systemControls;
        private bool DataLoaded;
        private bool _radioLoaded;
        private bool _videoLoaded;

        public HubPage()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;

            systemControls = SystemMediaTransportControls.GetForCurrentView();
            systemControls.ButtonPressed += SystemControls_ButtonPressed;

            systemControls.IsPlayEnabled = true;
            systemControls.IsPauseEnabled = true;
        }

        /// <summary>
        ///     Gets the NavigationHelper used to aid in navigation and process lifetime management.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        /// <summary>
        ///     Gets the DefaultViewModel. This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return defaultViewModel; }
        }

        /// <summary>
        ///     Populates the page with content passed during navigation.  Any saved state is also
        ///     provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event; typically <see cref="NavigationHelper" />
        /// </param>
        /// <param name="e">
        ///     Event data that provides both the navigation parameter passed to
        ///     <see cref="Frame.Navigate(Type, object)" /> when this page was initially requested and
        ///     a dictionary of state preserved by this page during an earlier
        ///     session.  The state will be null the first time a page is visited.
        /// </param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            LoadAudioStations();
            LoadVideoStations();
        }

        /// <summary>
        ///     Invoked when a HubSection header is clicked.
        /// </summary>
        /// <param name="sender">The Hub that contains the HubSection whose header was clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            HubSection section = e.Section;
            object group = section.DataContext;
            Frame.Navigate(typeof (SectionPage), ((SampleDataGroup) group).UniqueId);
        }

        /// <summary>
        ///     Invoked when an item within a section is clicked.
        /// </summary>
        /// <param name="sender">
        ///     The GridView or ListView
        ///     displaying the item clicked.
        /// </param>
        /// <param name="e">Event data that describes the item clicked.</param>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string streamUrl = ((Station) e.ClickedItem).StreamURL;
                MediaPlayer.Source = new Uri(streamUrl);
            }
            catch (Exception ex)
            {
                // TODO: Do something here to show error state
            }
        }

        #region NavigationHelper registration

        /// <summary>
        ///     The methods provided in this section are simply used to allow
        ///     NavigationHelper to respond to the page's navigation methods.
        ///     Page specific logic should be placed in event handlers for the
        ///     <see cref="Common.NavigationHelper.LoadState" />
        ///     and <see cref="Common.NavigationHelper.SaveState" />.
        ///     The navigation parameter is available in the LoadState method
        ///     in addition to page state preserved during an earlier session.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region Data Methods

        /// <summary>
        ///     Loads the audio stations from PVL and injects them into the view model
        /// </summary>
        private async void LoadAudioStations()
        {
            Station[] stations = await API.Instance.StationFetchTask("audio");
            var collection = new StationGroup
            {
                Title = "Radio",
                Stations = new ObservableCollection<Station>(stations)
            };
            DefaultViewModel["AudioStations"] = collection;
            _radioLoaded = true;

            if (_radioLoaded && _videoLoaded)
                DataLoaded = true;
        }

        private async void LoadVideoStations()
        {
            Station[] stations = await API.Instance.StationFetchTask("video");
            var collection = new StationGroup
            {
                Title = "Video",
                Stations = new ObservableCollection<Station>(stations)
            };
            DefaultViewModel["VideoStations"] = collection;
            _videoLoaded = true;

            if (_radioLoaded && _videoLoaded)
                DataLoaded = true;
        }

        #endregion

        #region Media Player events

        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs args)
        {
            StartPlayer();
        }

        private void MediaPlayer_OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MediaPlayer_OnCurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (MediaPlayer.CurrentState)
            {
                case MediaElementState.Playing:
                    systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                    break;
                case MediaElementState.Paused:
                    systemControls.PlaybackStatus = MediaPlaybackStatus.Paused;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Transport controls

        private void SystemControls_ButtonPressed(SystemMediaTransportControls sender,
            SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    StartPlayer();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    PausePlayer();
                    break;
                default:
                    break;
            }
        }

        private async void StartPlayer()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MediaPlayer.Play());
        }

        private async void PausePlayer()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MediaPlayer.Pause());
        }

        #endregion
    }
}