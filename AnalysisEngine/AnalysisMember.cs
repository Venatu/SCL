using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venatu.SCL.AnalysisEngine.Load;

namespace Venatu.SCL.AnalysisEngine
{
    class AnalysisMember
    {
        public List<ILoad> Loads;

        double GetMoment(double distance)
        {
            double total = 0;
            foreach(ILoad l in Loads)
            {
                total += l.GetMoment(distance);
            }
            return total;
        }

        double GetShear(double distance)
        {
            double total = 0;
            foreach (ILoad l in Loads)
            {
                total += l.GetShear(distance);
            }
            return total;
        }

        double GetDeflection(double distance)
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
