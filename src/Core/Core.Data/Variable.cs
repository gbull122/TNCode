using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace TNCode.Core.Data
{
    public class Variable : IVariable
    {
        private bool trimNans = true;

        public int Length { get; }

        public string Name { get; }

        public IReadOnlyCollection<object> Data { get; }

        public VariableValue VariableType { get; }

        public Variable(object[] rawData)
        {
            Name = FormatName(rawData[0].ToString());
            Data = ConvertArray(rawData, trimNans);

            if (CanConvertObjectArrayToDoubleArray(rawData))
            {
                VariableType = VariableValue.Numeric;
            }
            else
            {
                DateTime dateResult;
                if (DateTime.TryParseExact(rawData[1].ToString(), "dd/MM/yyyy hh:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult))
                {
                    Data = ConvertDoubleToDateTime(rawData, trimNans);
                    VariableType = VariableValue.DateTime;
                }

                if (Data == null)
                {

                    VariableType = VariableValue.Text;
                }
            }
            Length = Data.Count;
        }

        public IReadOnlyCollection<object> ConvertArray(object[] dataArray, bool trimNans)
        {
            var convertedArray = dataArray.ToList<object>();
            convertedArray.RemoveAt(0);

            if (trimNans)
                return TrimNansFromList(convertedArray);

            return convertedArray;
        }

        public IReadOnlyCollection<object> ConvertDoubleToDateTime(object[] testStringArray, bool trimNans)
        {
            var t = testStringArray.ToList<object>();
            t.RemoveAt(0);
            try
            {
                List<double> str = ((IEnumerable)t)
                                .Cast<object>()
                                .Select(x => DateTime.ParseExact(x.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture).ToOADate())
                                .ToList();
                if (trimNans)
                    return TrimNansFromList(str.Cast<object>().ToList<object>());

                return (str.Cast<object>().ToList().AsReadOnly());
            }
            catch
            {
                return null;
            }
        }

        public bool CanConvertObjectArrayToDoubleArray(object[] testStringArray)
        {
            var t = testStringArray.ToList<object>();
            t.RemoveAt(0);
            try
            {
                List<double> str = ((IEnumerable)t)
                                .Cast<object>()
                                .Select(x => double.Parse(x.ToString(), CultureInfo.InvariantCulture))
                                .ToList();

                return true;

            }
            catch
            {
                return false;
            }
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
            {
                name = "V_" + name;
            }

            return name.Replace(' ', '_');
        }
    }
}
