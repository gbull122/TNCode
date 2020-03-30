﻿using TNCode.Core.Data;

namespace TNCodeApp.Data.Events
{
    public enum DataSetChange
    {
        Added,
        Removed,
        Updated
    }

    public class DataSetEventArgs
    {
        public DataSet Data { get; set; }

        public DataSetChange Modification { get; set; }
    }
}
