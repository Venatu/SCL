using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL
{
    public interface IDesignElement
    {       
        void Analyze();

        double ULSUsage(double pos);

        double SLSUsage(double pos);
    }
}
