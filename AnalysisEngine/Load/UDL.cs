using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL.AnalysisEngine.Load
{
    class UDL : ILoad
    {
        double magnitude, length;

        public UDL(double magnitude, double length)
        {
            this.magnitude = magnitude;
            this.length = length;
        }

        public double GetDeflection(double distance)
        {
            return magnitude * distance / 24f * (Math.Pow(length, 3) - 2 * length * Math.Pow(distance, 2) + Math.Pow(distance, 3));
        }

        public double GetMoment(double distance)
        {
            return magnitude * distance / 2f * (length - distance);
        }

        public double GetShear(double distance)
        {
            return magnitude * (length / 2 - distance);
        }
    }
}
