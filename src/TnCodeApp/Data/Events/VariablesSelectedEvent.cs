using Prism.Events;
using System.Collections.Generic;

namespace TnCode.TnCodeApp.Data.Events
{
    public class VariablesSelectedEvent : PubSubEvent<Dictionary<string, ICollection<string>>>
    {
    }
}
