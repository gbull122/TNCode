﻿using Prism.Events;
using TNCode.Core.Data;

namespace TNCodeApp.Data.Events
{
    public class DataLoadedEvent : PubSubEvent<DataSet>
    {
    }
}
