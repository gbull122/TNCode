using System;
using System.Globalization;
using System.Windows.Controls;

namespace TnCode.TnCodeApp.Utilities.Validators
{
    public class ValidationDoubleRange: ValidationRule
    {
        public double Min { get; set; }

        public double Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double parameter = 0;

            try
            {
                if (((string)value).Length > 0)
                {
                    parameter = double.Parse((String)value);
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or "
                                             + e.Message);
            }

            if ((parameter < this.Min) || (parameter > this.Max))
            {
                return new ValidationResult(false,
                    "Please enter value in the range: "
                    + this.Min + " - " + this.Max + ".");
            }
            return new ValidationResult(true, null);
        }
    }
}

