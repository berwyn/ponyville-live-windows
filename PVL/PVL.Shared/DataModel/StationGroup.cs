﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PVL.DataModel
{
    class StationGroup : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _title;
        private ObservableCollection<StationViewModel> _stations;

        public string Title 
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged<string>();
            }
        }

        public ObservableCollection<StationViewModel> Stations 
        { 
            get { return _stations; }
            set
            {
                _stations = value;
                OnPropertyChanged<ObservableCollection<StationViewModel>>();
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
