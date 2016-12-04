using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL
{
    public class Project
    {
        public string ProjectID { get; set; }

        public string ProjectName { get; set; }

        public string Description { get; set; }

        public List<Revision> Revisions { get; set; }

        public Project()
        {
            ProjectID = "0000";
            ProjectName = "New Project";
            Description = "New project";

            Revisions = new List<Revision>();
            Revisions.Add(new Revision());
        }
    }
}
