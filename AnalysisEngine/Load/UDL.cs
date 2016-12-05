using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL.AnalysisEngine.Load
{
    public class UDL : ILoad
    {
        double magnitude, length;

        public UDL(double magnitude, double length)
        {
            this.magnitude = magnitude;
            this.length = length;
        }

        public double GetDeflection(double distance)
        {
            return magnitude*1000 * distance / 24f * (Math.Pow(length*1000, 3) - (2 * length*1000 * Math.Pow(distance*1000, 2)) + Math.Pow(distance*1000, 3));
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
