using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNCodeApp.R.Charts.Ggplot.Layer
{
    public class Facet
    {

        private string xVariable;
        private string yVariable;
        private bool isFreeSpace;
        private bool isFreeScale;

        public string XVariable
        {
            get { return xVariable; }
            set
            {
                xVariable = value;
            }
        }

        public string YVariable
        {
            get { return yVariable; }
            set
            {
                yVariable = value;
            }
        }

        public bool IsFreeScale
        {
            get { return isFreeScale; }
            set
            {
                isFreeScale = value;
            }
        }

        public bool IsFreeSpace
        {
            get { return isFreeSpace; }
            set
            {
                isFreeSpace = value;
            }
        }

        public Facet()
        {

        }

        public string PlotCommand()
        {
            StringBuilder result = new StringBuilder();
            if (string.IsNullOrEmpty(XVariable) && string.IsNullOrEmpty(YVariable))
            {
                return string.Empty;
            }


            result.Append("+facet_grid(");
            if (!string.IsNullOrEmpty(XVariable))
            {
                result.Append(XVariable + "~");
            }
            else
            {
                result.Append(".~");
            }

            if (!string.IsNullOrEmpty(YVariable))
            {
                result.Append(YVariable);
            }
            else
            {
                result.Append(".");
            }
            if (IsFreeScale)
            {
                result.Append(",scales=" + string.Format("\"{0}\"", "free"));
            }
            if (IsFreeSpace)
            {
                result.Append(",space=" + string.Format("\"{0}\"", "free"));
            }
            result.Append(")");
            return result.ToString();
        }
    }
}
