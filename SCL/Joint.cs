using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL
{
    public class Joint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public List<StructuralMember> ConnectedMembers;

        public Joint()
        {
            ConnectedMembers = new List<StructuralMember>();
        }
    }
}
