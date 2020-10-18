using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.Core.Utilities;
using TnCode.TnCodeApp.R.Controls;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class GeomViewModel: BindableBase
    {

        private readonly IXmlConverter xmlConverter;

        private ObservableCollection<VariableControl> geomControls;

        private List<string> variables;

        public DelegateCommand<string> SelectedGeomChangedCommand { get; private set; }

        public List<string> Geoms { get; }

        public List<string> Variables
        {
            get => variables;
            set
            {
                variables = value;
                RaisePropertyChanged(nameof(Variables));
            }
        }

        public ObservableCollection<VariableControl> GeomControls
        {
            get { return geomControls; }
            set
            {
                geomControls = value;
                RaisePropertyChanged(nameof(GeomControls));
            }
        }

        public GeomViewModel(IXmlConverter converter)
        {
            xmlConverter = converter;

            SelectedGeomChangedCommand = new DelegateCommand<string>(GeomChanged);

            Geoms = Enum.GetNames(typeof(Ggplot.Geoms)).ToList();
            Geoms.Remove("tile");

            geomControls = new ObservableCollection<VariableControl>();
            variables = new List<string>();
        }

        private void GeomChanged(string obj)
        {
            throw new NotImplementedException();
        }

        private Aesthetic MergeAesthetics(Aesthetic aestheticFromFile)
        {
            //if (SelectedLayer.Aes == null)
            //    return null;

            var mergedAesthetic = new Aesthetic();

            mergedAesthetic.DefaultStat = aestheticFromFile.DefaultStat;
            mergedAesthetic.DefaultPosition = aestheticFromFile.DefaultPosition;

            //foreach (var aesValue in aestheticFromFile.AestheticValues)
            //{
            //    if (SelectedLayer.Aes.DoesAestheticContainValue(aesValue.Name))
            //    {
            //        var existingValue = SelectedLayer.Aes.GetAestheticValueByName(aesValue.Name);
            //        aesValue.Entry = existingValue.Entry;
            //    }
            //    mergedAesthetic.AestheticValues.Add(aesValue);
            //}
            return mergedAesthetic;
            //SelectedLayer.Aes = mergedAesthetic;

            //RaisePropertyChanged(string.Empty);
        }
    }
}
