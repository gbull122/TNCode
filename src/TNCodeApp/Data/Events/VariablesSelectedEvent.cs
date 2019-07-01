using Prism.Events;
using System.Collections.Generic;
using TNCode.Core.Data;

namespace TNCodeApp.Data.Events
{
    public class VariablesSelectedEvent : PubSubEvent<IList<object>>
    {
    }
}
