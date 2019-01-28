using Microsoft.R.Host.Client;
using System;
using System.Threading.Tasks;

namespace ModuleR.R
{
    public class RManager : IRManager
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
