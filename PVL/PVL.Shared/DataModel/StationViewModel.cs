using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PVL.DataModel
{
    class StationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Station _station;
        private NowPlayingInfo _nowPlaying;

        public Station Station
        {
            get { return _station; }
            set
            {
                _station = value;
                OnPropertyChanged<Station>();
            }
        }
        public NowPlayingInfo NowPlaying
        {
            get { return _nowPlaying; }
            set
            {
                _nowPlaying = value;
                OnPropertyChanged<NowPlayingInfo>();
            }
        }

        private void OnPropertyChanged<T>([CallerMemberName] string caller = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
