using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL
{
    public class Revision
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }

        public List<Option> AnalysisObjects { get; set; }

        public Revision()
        {
            Name = "Initial";
            Date = DateTime.Now.ToShortDateString();

            AnalysisObjects = new List<Option>();
        }

    }
}
