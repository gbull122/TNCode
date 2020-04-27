using System.Collections.Generic;

namespace TnCode.Core.Data
{
    public interface IVariable
    {
        IReadOnlyCollection<object> Values { get; }
        int Length { get; }
        string Name { get; }
    }
}
