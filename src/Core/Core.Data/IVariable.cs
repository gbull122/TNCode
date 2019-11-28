using System.Collections.Generic;

namespace TNCode.Core.Data
{
    public interface IVariable
    {
        IReadOnlyCollection<object> Values { get; }
        int Length { get; }
        string Name { get; }
        DataType Data { get; }
        bool IsSelected { get; set; }
    }
}
