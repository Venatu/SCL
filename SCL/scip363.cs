using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL
{
    public class scip363
    {
        static Dictionary<string, float[]> UKB;        

        public static Dictionary<string, float[]> GetUKB()
        {
            if(UKB == null)
            {
                var lines = Properties.Resources.UKB.Replace("\n","").Split('\r');//(new[] { '\r', '\n' });
                UKB = new Dictionary<string, float[]>();
                for (int i = 7; i < lines.Length; i++)
                {
                    var sections = lines[i].Split(',');

                    float[] data = new float[sections.Length - 1];
                    if (sections[1] == "")
                    {
                        data[0] = 0;
                    } else
                    {
                        data[0] = 1;
                    }                     
                    for (int j = 2; j < sections.Length; j++)
                    {
                        data[j - 1] = float.Parse(sections[j]);
                    }

                    UKB.Add(sections[0], data);
                }                
            }

            return UKB;
        }

        public static float Lookup(SteelType stype, string Element, Property property)
        {
            float result = 0;

            switch (stype)
            {
                case SteelType.UKB:
                    var data = GetUKB();
                    var line = data[Element];
                    result = line[(int)property];
                    break;             

            }              

            return result;
            
        }        

        public enum Property
        {
            Unusual,
            SelfWeight,
            Height,
            Width,
            WebThickness,
            FlangeThickness,
            RootRadius,
            DepthBetweenFillets,
            WebBucklingRatio,
            FlangeBucklingRation,
            EndClearance,
            N,
            n,
            SurfaceAreaPerMetre,
            SurfaceAreaPerTonne,
            Iyy,
            Izz,
            WelY,
            WelZ,
            WplY,
            WplZ,
            BucklingParameter,
            TorsionalIndex,
            WarpingConstant,
            TorsionalConstant,
            AreaOfSection
        }

    }

    public enum SteelType
    {
        UKB
    }
}
