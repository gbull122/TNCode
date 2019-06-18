using Prism.Events;
using System;
using System.Collections.Generic;

namespace TNCodeApp.Data.Events
{
    public class VariablesSelectedEvent : PubSubEvent<IList<object>>
    {
    }
}
