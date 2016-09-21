using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL
{
    public class Document
    {
        private Dictionary<string, StructuralMember> Members;
        private Dictionary<int, Joint> Joints;
        private int JointID;

        private List<Type> GravityAnalysis;

        public Document()
        {
            Members = new Dictionary<string, StructuralMember>();
            Joints = new Dictionary<int, Joint>();
            JointID = 0;

            GravityAnalysis = new List<Type>();
            Populate();
        }

        private void Populate()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
            {
                var ass = Assembly.LoadFile(dll);
                var gravity = from type in ass.GetTypes() where typeof(IAnalysis).IsAssignableFrom(type) select type;
                foreach (Type t in gravity)
                {
                    GravityAnalysis.Add(t);
                }
            }        
        }

        public void AddMember(StructuralMember m, Joint start, Joint end)
        {
            m.Start = GetMatchingJoint(start);
            m.End = GetMatchingJoint(end);

            Members.Add(m.ID, m);
        }

        public void Calculate(AnalysisType atype)
        {
            IAnalysis analysis = null;

            switch (atype)
            {
                case AnalysisType.Gravity:
                    //TODO: Add proper engine selector
                    analysis = (IAnalysis) Activator.CreateInstance(GravityAnalysis[0]);
                    break;                
            }

            analysis.Analyse(this);
        }


        private Joint GetMatchingJoint(Joint test)
        {
            const double Tolerance = 1;
            const double ToleranceSquared = Tolerance * Tolerance;

            foreach (Joint j in Joints.Values)
            {
                if(ToleranceSquared > Math.Pow(test.X - j.X,2) + Math.Pow(test.Y - j.Y, 2) + Math.Pow(test.Z - j.Z, 2))
                {                    
                    return j;
                }
            }

            Joints.Add(JointID, test);
            JointID++;

            return test;
        }
    }

    public enum AnalysisType
    {
        Gravity,
    }
}
