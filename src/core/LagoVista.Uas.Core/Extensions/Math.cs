using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas
{
    public static class MathExtensions
    {
        public static double ToDegrees(this double value)
        {
            return (value * 180.0) / Math.PI;
        }

        public static float ToDegrees(this float value)
        {
            return  Convert.ToSingle((value * 180.0f) / Math.PI);
        }
    }
}
