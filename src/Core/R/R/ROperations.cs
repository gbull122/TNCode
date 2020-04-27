using RDotNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TnCode.Core.R
{
    public class ROperations //: IROperations
    {
        private REngine rEngine;

        public ROperations(REngine eEng)
        {
            rEngine = eEng;
        }

        public bool IsHostRunning()
        {
            return rEngine.IsRunning;
        }

        public async Task StartRAsync()
        {
            //await rEngine.StartHostAsync(rHostSessionCallback);
        }

        public async Task ExecuteCommandAsync(string command)
        {
            rEngine.Evaluate(command);
        }

        public async Task<DataFrame> GetDataFrameAsync(string name)
        {
            DataFrame requiredFrame = (DataFrame)rEngine.GetSymbol(name).AsDataFrame();
            return requiredFrame;
        }

        //public async Task<RSessionOutput> ExecuteAndOutputAsync(string command)
        //{
        //    return await rEngine.ExecuteAndOutputAsync(command);
        //}

        //public async Task<List<object>> GetListAsync(string command)
        //{
        //    return await rEngine.GetListAsync(command);
        //}

        //public async Task<string> EvaluateAsync<T>(string command)
        //{
        //    return await rEngine.EvaluateAsync<string>(command);
        //}

        //public async Task ExecuteAsync(string command)
        //{
        //    await rEngine.ExecuteAsync(command);
        //}

        //public async Task CreateDataFrameAsync(string name, DataFrame dataFrame)
        //{
        //    logger.Log("Creating DataFrame: " + name, Category.Info, Priority.None);
        //    await rEngine.CreateDataFrameAsync(name, dataFrame);
        //}
    }
}
