using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL
{
    public class StructuralMember
    {
        public string ID { get; }

        public double YoungsModulus { get; set; }

        private Joint _Start, _End;

        internal Joint Start
        {
            get
            {
                return _Start;
            }
            set
            {
                value.ConnectedMembers.Add(this);
                _Start = value;
            }
        }
            
        internal Joint End
        {
            get
            {
                return _End;
            }
            set
            {
                value.ConnectedMembers.Add(this);
                _End = value;
            }
        }

        public StructuralMember(string id)
        {
            this.ID = id;            
        }
    }
}
