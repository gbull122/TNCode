using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNCode.Core.Data
{
    public interface IVariable
    {

        IReadOnlyCollection<object> Data { get; }
        int Length { get; }
        string Name { get; }
        VariableValue VariableType { get; }
        bool CanConvertObjectArrayToDoubleArray(object[] testStringArray);
        IReadOnlyCollection<object> ConvertArray(object[] dataArray, bool trimNans);
        IReadOnlyCollection<object> ConvertDoubleToDateTime(object[] testStringArray, bool trimNans);
        string FormatName(string name);
        IReadOnlyCollection<object> TrimNansFromList(List<object> data);
    }
}
