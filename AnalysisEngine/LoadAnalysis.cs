using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venatu.SCL.AnalysisEngine.Load;

namespace Venatu.SCL.AnalysisEngine
{
    public class LoadAnalysis
    {
        public const double G = 9.81;

        public List<ILoad> Loads;

        public LoadAnalysis()
        {
            Loads = new List<ILoad>();
        }

        public double GetMoment(double distance)
        {
            double total = 0;
            foreach(ILoad l in Loads)
            {
                total += l.GetMoment(distance);
            }
            return total;
        }

        public double GetShear(double distance)
        {
            double total = 0;
            foreach (ILoad l in Loads)
            {
                total += l.GetShear(distance);
            }
            return total;
        }

        public double GetDeflection(double distance)
        {
            double total = 0;
            foreach (ILoad l in Loads)
            {
                total += l.GetDeflection(distance);
            }
            return total;
        }
    }
}
