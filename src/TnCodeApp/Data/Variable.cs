using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace TnCode.TnCodeApp.Data
{
    public class Variable : IVariable, INotifyPropertyChanged
    {
        public enum Format
        {
            Unknown,
            Text,
            Discrete,
            Continuous,
            DateTime
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int Length { get; }

        public string Name { get; }

        public IReadOnlyCollection<object> Values { get; }

        public Format DataFormat { get; }
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }


        public Variable(object[] rawData)
        {
            Name = FormatName(rawData[0].ToString());

            var conversionResult = CanConvertObjectArrayToDoubleArray(rawData);
            if (conversionResult.Item1)
            {
                Values = conversionResult.Item2;
                DataFormat = Format.Continuous;
            }
            else
            {
                DateTime dateResult;
                if (DateTime.TryParseExact(rawData[1].ToString(), "dd/MM/yyyy hh:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult))
                {
                    Values = ConvertDoubleToDateTime(rawData);
                    DataFormat = Format.DateTime;
                }
                else
                {
                    Values = ArrayToCollection(rawData, true);
                    DataFormat = Format.Text;
                }
            }
            Length = Values.Count;
        }

        public IReadOnlyCollection<object> ArrayToCollection(object[] dataArray, bool trimNans)
        {
            var convertedArray = dataArray.ToList<object>();
            convertedArray.RemoveAt(0);

            if (trimNans)
                return TrimNansFromList(convertedArray);

            return convertedArray;
        }

        public IReadOnlyCollection<object> ConvertDoubleToDateTime(object[] testStringArray)
        {
            var t = testStringArray.ToList<object>();
            t.RemoveAt(0);
            try
            {
                List<double> str = ((IEnumerable)t)
                                .Cast<object>()
                                .Select(x => DateTime.ParseExact(x.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture).ToOADate())
                                .ToList();

                return (str.Cast<object>().ToList().AsReadOnly());
            }
            catch
            {
                return null;
            }
        }

        public Tuple<bool, IReadOnlyCollection<object>> CanConvertObjectArrayToDoubleArray(object[] testStringArray)
        {
            IReadOnlyCollection<object> convertedArray = null;
            var didDataConvert = false;

            var t = testStringArray.ToList<object>();
            t.RemoveAt(0);
            try
            {
                List<double> str = ((IEnumerable)t)
                                .Cast<object>()
                                .Select(x => double.Parse(x.ToString(), CultureInfo.InvariantCulture))
                                .ToList();

                convertedArray = str.Cast<object>().ToList().AsReadOnly();
                didDataConvert = true;

            }
            catch
            {
                ///Do nothing
            }

            return new Tuple<bool, IReadOnlyCollection<object>>(didDataConvert, convertedArray);
        }

        public IReadOnlyCollection<object> TrimNansFromList(List<object> data)
        {
            while (data[data.Count - 1].ToString() == "NaN")
            {
                data.RemoveAt(data.Count - 1);
            }
            return data.AsReadOnly();
        }

        public string FormatName(string name)
        {
            if (Regex.IsMatch(name, @"^\d"))
                name = "V_" + name;

            return name.Replace(' ', '_');
        }
    }
}
