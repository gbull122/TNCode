using Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Schema;
using TnCode.Core.R.Functions;

namespace TnCode.TnCodeApp.R
{
    public class RFunctionService
    {

        private IRService rService;
        private ILoggerFacade logger;
        private List<RFunctionCollection> loadedFunctions;
        private RFunction _currentFunction = null;
        private RFunctionInput _lastInput;

        public RFunctionService(IRService rSer, ILoggerFacade log)
        {
            rService = rSer;
            logger = log;
            loadedFunctions = new List<RFunctionCollection>();
            LoadFunctions();
            _lastInput = new RFunctionInput()
            {
                //Name = "Void",
                InputType = "None"
            };
        }

        internal List<RFunctionCollection> LoadedFunctions
        {
            get { return loadedFunctions; }
        }

        internal void DoFunction(string library, string function)
        {
            var aFunction = GetFunction(library, function);

            if (aFunction != null)
            {
                //if (Globals.ThisAddIn.DataUpdateOn)
                //{
                //    RunAutoUpdateFunction(aFunction);
                //}
                //else
                //{
                    //RunFunction(aFunction);
                //}


            }
        }

        private void RunFunction(RFunction aFunction,List<string> variables)
        {
            if (!aFunction.Input[0].InputType.Equals("None"))
            {
                _currentFunction = aFunction;
                //if the data hasn't changed and the inputs are the same format
                if (aFunction.Input[0].Equals(_lastInput))
                {
                    _lastInput = aFunction.Input[0];
                    BuildFunctionForm(aFunction, variables);
                }
                else
                {
                    string message = "Selected data is not in the right format for the function.";
                    logger.Log(message, Category.Info, Priority.Medium);
                }
            }
            else
            {
                BuildFunctionForm(aFunction);
            }

        }

        private void RunAutoUpdateFunction(RFunction aFunction)
        {
            if (!aFunction.Input[0].InputType.Equals("None"))
            {
                _currentFunction = aFunction;
                //if the data hasn't changed and the inputs are the same format
                if (aFunction.Input[0].Equals(_lastInput))
                {
                    _lastInput = aFunction.Input[0];
                    //BuildFunctionForm(aFunction, _currentSelection.Headers);
                }
                else
                {
                    //Get the selected data.
                    //_currentSelection = Globals.ThisAddIn.GetSelectedData();
                    //if (_currentSelection != null)
                    //{
                    //    //
                    //    if (IsDataValid(aFunction.Input[0]))
                    //    {
                    //        //Get the data into R
                    //        rService.DoInputs(aFunction, _currentSelection);
                    //        BuildFunctionForm(aFunction, _currentSelection.Headers);
                    //    }
                    //    else
                    //    {
                    //        //Globals.ThisAddIn.ShowErrorMessage("Selected data is not in the right format for the function.");
                    //    }
                    //}
                    //else
                    //{
                    //    //Globals.ThisAddIn.ShowErrorMessage("No data is selected.");
                    //}
                }
            }
            else
            {
                BuildFunctionForm(aFunction);
            }

        }

        private bool IsDataValid(RFunctionInput fIn)
        {
            //switch (fIn.InputType)
            //{
            //    case "Vector":
            //        if (_currentSelection.AllColumns.Count == fIn.Columns.Count)
            //        {
            //            for (int col = 0; col < _currentSelection.AllColumns.Count; col++)
            //            {
            //                var requiredType = fIn.Columns[col];
            //                if (!requiredType.Equals("Any"))
            //                {
            //                    var selectedType = _currentSelection.AllColumns[col].ColumnData.ToString();

            //                    if (!selectedType.Equals(requiredType))
            //                        return false;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //        break;

                //case "DataFrame":
                //    if (_currentSelection.AllColumns.Count < 2)
                //        return false;
                //    break;
            //}
            return true;
        }

        private void BuildFunctionForm(RFunction aFunction, List<string> variables)
        {
            //Func<RFunction, string[], FunctionFormUserControl> formCreator = (x, y) => new FunctionFormUserControl();
            //var taskpane = TaskPaneManager.GetTaskPane(aFunction, variables.ToArray(), formCreator);
            //taskpane.Visible = true;
        }

        private void BuildFunctionForm(RFunction aFunction)
        {
            //Func<RFunction, string[], FunctionFormUserControl> formCreator = (x, y) => new FunctionFormUserControl();
            //var taskpane = TaskPaneManager.GetTaskPane(aFunction, new string[0], formCreator);
            //taskpane.Visible = true;
        }

        private void UpdateFunctionData()
        {
            //TaskPaneManager.UpdatePane(_currentSelection.Headers.ToArray());
        }

        public RFunction GetFunction(string library, string function)
        {
            foreach (var funcs in loadedFunctions)
            {
                if (funcs.Name.Equals(library))
                {
                    foreach (var fun in funcs.Functions)
                    {
                        if (fun.Name.Equals(function))
                        {
                            return fun;
                        }
                    }
                }
            }
            return null;
        }

        private void LoadFunctions()
        {
            loadedFunctions.Clear();

            //if (string.IsNullOrEmpty(Properties.Settings.Default.FunctionFolder))
            //{
            //    Properties.Settings.Default.FunctionFolder =
            //        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EasyER");
            //    Properties.Settings.Default.Save();
            //}

            //if (GetXmlFiles(Properties.Settings.Default.FunctionFolder).Count == 0)
            //{
            //    var installFolder = Path.GetDirectoryName(
            //        new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            //    foreach (string f in GetXmlFiles(installFolder))
            //    {
            //        if (f.StartsWith("easyer"))
            //            File.Copy(Path.Combine(installFolder, f), Path.Combine(Properties.Settings.Default.FunctionFolder, f));
            //    }
            //}
            //LoadFunctions(Properties.Settings.Default.FunctionFolder);
        }

        private List<string> GetXmlFiles(string folder)
        {
            List<string> files = new List<string>();
            var xmlFiles = Directory.GetFiles(folder, "*.xml");
            foreach (var file in xmlFiles)
            {
                files.Add(Path.GetFileName(file));
            }

            return files;
        }

        private void LoadFunctions(string functionFolder)
        {
            if (Directory.Exists(functionFolder))
            {
                var xmlFiles = Directory.GetFiles(functionFolder, "*.xml");
                foreach (var file in xmlFiles)
                {
                    LoadFunction(file);
                }
            }
        }

        /// <summary>
        /// Load a function file and convert into R function
        /// </summary>
        /// <param name="path"></param>
        private void LoadFunction(string path)
        {
            if (File.Exists(path))
            {
                //validate xml deal with errors
                //XmlSchemaValidator.TestXML(path);
                //if (XmlSchemaValidator.IsValid)
                //{
                //    XmlReader reader = XmlReader.Create(path);
                //    XmlDocument document = new XmlDocument();
                //    document.Load(reader);
                //    reader.Close();
                //    var newFc = XmlConverter.ToObject<RFunctionCollection>(document.InnerXml);
                //    if (newFc != null)
                //    {
                //        List<Tuple<string, string, string, bool>> packResults = rService.CheckPackages(newFc.Packages);

                //        foreach (Tuple<string, string, string, bool> pack in packResults)
                //        {
                //            rService.InstallPackage(pack.Item1);

                //        }
                //        loadedFunctions.Add(newFc);

                //    }
                //}
                //else
                //{
                //    //Report xml not valid
                //    System.Windows.Forms.MessageBox.Show("Failed to load the Sharp-R function file " + Path.GetFileName(path) + " as is does not follow the correct XML schema. " + XmlSchemaValidator.Error);
                //}
            }
        }

        /// <summary>
        /// Take a list of missing libraries and summarise as text.
        /// </summary>
        /// <param name="libraries"></param>
        /// <returns>String to display to the user.</returns>
        internal string FormatMissingLibraries(List<Tuple<string, string, string, bool>> libraries)
        {
            StringBuilder miss = new StringBuilder();
            miss.AppendLine("The following packages are not installed:");
            StringBuilder ver = new StringBuilder();
            ver.AppendLine("The following packages need updating:");
            foreach (Tuple<string, string, string, bool> lib in libraries)
            {
                if (lib.Item4)
                {
                    miss.AppendLine(lib.Item1 + " " + lib.Item2 + " " + lib.Item3);
                }
                else
                {
                    ver.AppendLine(lib.Item1 + " " + lib.Item2 + " " + lib.Item3);
                }
            }
            string result = string.Empty;
            if (ver.Length > 37)
            {
                result = ver.ToString();
            }

            if (miss.Length > 41)
            {
                result = result + "\n" + miss.ToString();
            }

            return result;
        }

    }
}