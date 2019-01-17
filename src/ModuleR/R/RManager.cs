using Microsoft.R.Host.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleR.R
{
    public class RManager
    {
        private IRHostSession rSession;
        private RHostSessionCallback rHostSessionCallback;

        public RManager()
        {

        }

        public async Task<bool> InitialiseAsync()
        {
            try
            {
                rSession = RHostSession.Create("TNCode");

                await rSession.StartHostAsync(rHostSessionCallback);
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
    }
}
