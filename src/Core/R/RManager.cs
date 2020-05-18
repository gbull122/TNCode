using RDotNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TnCode.Core.Data;

namespace TnCode.Core.R
{
    public class RManager : IRManager
    {
        private REngine rEngine;
        private string workingDirectory = string.Empty;
        private string rHomePath = "C:\\Program Files\\R\\R-4.0.0";
        private string rInstallationPath = "C:\\Program Files\\R\\R-4.0.0\\bin\\x64";

        public bool IsRRunning => false;

        public string RHome
        {
            get { return rHomePath; }
            set { rHomePath = value; }
        }

        public string RPath
        {
            get { return rInstallationPath; }
            set { rInstallationPath = value; }
        }

        public RManager(string tempPath)
        {
            workingDirectory = tempPath;
        }

        public Task<bool> DataSetToRAsDataFrameAsync(DataSet data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GenerateGgplotAsync(string ggplotCommand)
        {
            throw new NotImplementedException();
        }

        public Task<DataSet> GetDataFrameAsDataSetAsync(string name)
        {
            throw new NotImplementedException();
        }

        public bool InitialiseAsync()
        {
            var result = true;
            try
            {
                //if (!string.IsNullOrEmpty(rHomePath)
                //    && !string.IsNullOrEmpty(rInstallationPath))
                //{
                //    REngine.SetEnvironmentVariables(rInstallationPath, rHomePath);
                //}
                rEngine = REngine.GetInstance();
                //SetWorkingDirectory(ConverPathToR(workingDirectory));
                result = true;
            }
            catch (Exception ex)
            {
                //startUpError = ex.Message;
                result = false;
            }
            return result;

      
        }

        public Task<bool> IsDataFrame(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<object>> ListWorkspaceItems()
        {
            throw new NotImplementedException();
        }

        public Task LoadRWorkSpace(IProgress<string> progress, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task LoadToTempEnv(string fullFileName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTempEnviroment()
        {
            throw new NotImplementedException();
        }

        public Task<string> RHomeFromConnectedRAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> RPlatformFromConnectedRAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> RVersionFromConnectedRAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<object>> TempEnvObjects()
        {
            throw new NotImplementedException();
        }

        private string ConverPathToR(string path)
        {
            string temp = path.Replace('\\', '/');
            return string.Format("\"{0}\"", temp);
        }

        private void SetWorkingDirectory(string rPath)
        {
            rEngine.Evaluate("setwd(" + rPath + ")");
        }
    }
}
