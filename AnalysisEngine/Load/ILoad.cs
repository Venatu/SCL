using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL.AnalysisEngine.Load
{
    public interface ILoad
    {
        double GetShear(double distance);

        double GetMoment(double distance);

        double GetDeflection(double distance);
    }
}
