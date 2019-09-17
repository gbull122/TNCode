﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using TNCode.Core.Data;

namespace TNCodeApp.Data
{
    public interface IDataSetsManager
    {
        ObservableCollection<IDataSet> DataSets { get; set; }
        Dictionary<string, ICollection<string>> SelectedData();
    }
}