using System.Collections.Generic;

namespace TNCode.Core.Data
{
    public interface IDataSet
    {
        string Name { get; set; }
        List<string> VariableNames();
        List<string> ObservationNames { get; }
        List<IVariable> Variables { get; }
        bool AllColumnsEqual();
        IReadOnlyList<IReadOnlyList<object>> RawData();
    }
}
