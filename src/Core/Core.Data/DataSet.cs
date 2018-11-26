using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNCode.Core.Data
{
    public class DataSet : IDataSet
    {
        public string Name => throw new NotImplementedException();

        public List<string> VariableNames => throw new NotImplementedException();

        public List<string> ObservationNames => throw new NotImplementedException();

        public List<IVariable> Variables => throw new NotImplementedException();

        public bool AllColumnsEqual()
        {
            throw new NotImplementedException();
        }
    }
}
