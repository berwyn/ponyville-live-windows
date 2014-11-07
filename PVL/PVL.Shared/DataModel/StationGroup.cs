using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PVL.DataModel
{
    class StationGroup
    {

        public string Title { get; set; }
        public ObservableCollection<Station> Stations { get; set; }

    }
}
