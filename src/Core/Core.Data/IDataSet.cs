using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNCode.Core.Data
{
    public interface IDataSet
    {
        string Name { get; }
        List<string> VariableNames { get; }
        List<string> ObservationNames { get; }
        List<IVariable> Variables { get; }
        bool AllColumnsEqual();
    }
}
