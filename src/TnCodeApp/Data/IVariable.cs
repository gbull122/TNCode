using System.Collections.Generic;

namespace TnCode.TnCodeApp.Data
{
    public interface IVariable
    {
        IReadOnlyCollection<object> Values { get; }
        int Length { get; }
        string Name { get; }
        string Id { get; }
        Variable.Format DataFormat { get; }
        bool IsSelected { get; set; }
    }
}
