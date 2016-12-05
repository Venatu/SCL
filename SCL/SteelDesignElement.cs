using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venatu.SCL.AnalysisEngine;
using Venatu.SCL.AnalysisEngine.Load;
using static Venatu.SCL.scip363;

namespace Venatu.SCL
{
    public class SteelDesignElement : IDesignElement
    {
        public const double Steps = 0.025;
        public const double E = 210000;

        public delegate void AnalysisHandler(SteelDesignElement source, EventArgs e);
        public event AnalysisHandler AnalysisComplete;

        private LoadAnalysis LoadCombination;

        public SteelType Type { get; set; }

        public string SectionProfile
        {
            get
            {
                return _SectionProfile;
            }
            set
            {
                _SectionProfile = value;
                Rebuild();
            }
        }

        string _SectionProfile;
        double WplY, SectionArea, Iyy;

        public double Length { get { return _Length; } set { _Length = value; Rebuild(); } }
        private double _Length;

        public double Grade { get { return _Grade; } set { _Grade = value; Rebuild(); } }
        private double _Grade;

        public double maxULS { get; set; }
        public double maxSLS { get; set; }
        public double maxULSPos { get; set; }
        public double maxSLSPos { get; set; }

        private double SelfWeight;

        public SteelDesignElement()
        {
            LoadCombination = new LoadAnalysis();
        }

        public void Analyze()
        {
            double position = 0;
            maxULS = 0;
            maxSLS = 0;
            maxULSPos = 0;
            maxSLSPos = 0;
            while (position <= Length)
            {
                var result = ULSUsage(position);
                if (result > maxULS)
                {
                    maxULS = result;
                    maxULSPos = position;
                }

                result = SLSUsage(position);
                if (result > maxSLS)
                {
                    maxSLS = result;
                    maxSLSPos = position;
                }

                position += Steps;
            }

            maxULS = Math.Round(maxULS, 3);
            maxSLS = Math.Round(maxSLS, 3);

            AnalysisComplete(this, null);
        }

        public double ULSUsage(double pos)
        {
            double moment, shear;

            moment = LoadCombination.GetMoment(pos);
            shear = LoadCombination.GetShear(pos);

            var MplY = WplY * Grade;
            var Ved = SectionArea * Grade;

            return Math.Max(moment / MplY, shear / Ved);
        }

        public double SLSUsage(double pos)
        {
            double deflection = LoadCombination.GetDeflection(pos) / (E * Iyy);
            var allowable = Length * 1000 / 360;
            return deflection / allowable;
        }

        public void Rebuild()
        {
            //Build properties
            SelfWeight = scip363.Lookup(Type, SectionProfile, Property.SelfWeight);
            WplY = scip363.Lookup(Type, SectionProfile, Property.WelY) * 1000;
            SectionArea = scip363.Lookup(Type, SectionProfile, Property.AreaOfSection) * 100;
            Iyy = scip363.Lookup(Type, SectionProfile, Property.Iyy) * 10000;

            //Build loads
            if (LoadCombination.Loads.Count <= 0)
            {
                LoadCombination.Loads.Add(new UDL(SelfWeight * LoadAnalysis.G / 1000, Length));
            } else
            {
                LoadCombination.Loads[0] = new UDL(SelfWeight * LoadAnalysis.G / 1000, Length);
            }

            Analyze();
        }

        public override string ToString()
        {
            return SectionProfile + " " + Type;
        }
    }
}
