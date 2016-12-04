using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL
{
    public class Option
    {
        public string Name { get; set; }
        public double Utilisation { get; set; }

        public List<IDesignElement> Elements { get; set; }

        public Option()
        {
            Elements = new List<IDesignElement>();            
        }
    }
}
