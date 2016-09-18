using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL.AnalysisEngine
{
    public class Model
    {
        MatrixBuilder<double> M = Matrix<double>.Build;
        VectorBuilder<double> V = Vector<double>.Build;

        Matrix<double> JointCoordinates, SupportData, MemberData, LoadData;
        Vector<double> ElasticModulus, CrossSectionalArea, JointLoads;

        public Vector<double> Displacements, Reactions;

        public Model(int NumberOfJoints, int NumberOfSupports, int NumberOfMaterials, int NumberOfSections, int NumberOfMembers, int NumberOfLoads)
        {
            JointCoordinates = M.Dense(NumberOfJoints, 2);
            SupportData = M.Dense(NumberOfSupports, 3);
            MemberData = M.Dense(NumberOfMembers, 4);
            LoadData = M.Dense(NumberOfLoads, 2);

            ElasticModulus = V.Dense(NumberOfMaterials);
            CrossSectionalArea = V.Dense(NumberOfSections);
            JointLoads = V.Dense(NumberOfLoads);
        }

        public void SetJointCoordinate(int JointID, double x, double y)
        {
            JointCoordinates[JointID, 0] = x;
            JointCoordinates[JointID, 1] = y;
        }

        public void SetSupport(int Support, int Joint, double FixedX, double FixedY)
        {
            SupportData[Support, 0] = Joint;
            SupportData[Support, 1] = FixedX;
            SupportData[Support, 2] = FixedY;
        }

        public void SetMember(int MemberID, double StartJointID, double EndJointID, double MaterialID, double SectionID)
        {
            MemberData[MemberID, 0] = StartJointID;
            MemberData[MemberID, 1] = EndJointID;
            MemberData[MemberID, 2] = MaterialID;
            MemberData[MemberID, 3] = SectionID;
        }

        public void SetMaterial(int MaterialID, double ElasticMod)
        {
            ElasticModulus[MaterialID] = ElasticMod;
        }

        public void SetSection(int SectionID, double Area)
        {
            CrossSectionalArea[SectionID] = Area;
        }

        public void SetLoad(int LoadID, double JointID, double Vx, double Vy)
        {
            JointLoads[LoadID] = JointID;
            LoadData[LoadID, 0] = Vx;
            LoadData[LoadID, 1] = Vy;
        }

        public void Calculate()
        {
            int NoR = NumberOfRestraints();
            int NDoF = 2 * JointCoordinates.RowCount - NoR; //Calulate number of degrees of freedom of the model
            Reactions = V.Dense(NoR);

            //Create the helper matrix
            Vector<double> StructureCoordinates = AssignStructureNumbers(NDoF);

            //Create S stiffness matrix
            Matrix<double> S = M.Dense(NDoF, NDoF);

            //Populate S matrix
            for (int i = 0; i < MemberData.RowCount; i++)
            {
                //Generate in the member stiffnes matrix in global coordinates
                var GK = BuildStiffnesMatrix(i);

                //Store the member matrix in the global array
                for (int j = 0; j < GK.RowCount; j++)
                {
                    int startJointID = (int)MemberData[i, 0];
                    int endJointID = (int)MemberData[i, 1];

                    double N1;
                    if (j < 2)
                    {
                        N1 = (startJointID) * 2 + j;
                    }
                    else
                    {
                        N1 = (endJointID) * 2 + (j - 2);
                    }
                    int jointCode1 = (int)StructureCoordinates[(int)N1];

                    if (jointCode1 < NDoF)
                    {
                        for (int k = 0; k < GK.ColumnCount; k++)
                        {
                            double N2;
                            if (k < 2)
                            {
                                N2 = (startJointID) * 2 + k;
                            }
                            else
                            {
                                N2 = (endJointID) * 2 + (k - 2);
                            }
                            int jointCode2 = (int)StructureCoordinates[(int)N2];

                            if (jointCode2 < NDoF)
                            {
                                S[jointCode1, jointCode2] = S[jointCode1, jointCode2] + GK[j, k];
                            }
                        }
                    }
                }
            }

            //Stiffness matrix created, build load matrix
            var P = V.Dense(NDoF);
            for (int i = 0; i < JointLoads.Count; i++)
            {
                int jointNumber = (int)JointLoads[i];

                int XCoord = (int)StructureCoordinates[(int)(jointNumber) * 2];
                int YCoord = (int)StructureCoordinates[(int)(jointNumber) * 2 + 1];

                if (XCoord < NDoF)
                {
                    P[XCoord] = LoadData[i, 0];
                }

                if (YCoord < NDoF)
                {
                    P[YCoord] = LoadData[i, 1];
                }
            }

            //Solve for global displacements
            Displacements = S.Solve(P);

            

            //Iterate over each member
            for (int i = 0; i < MemberData.RowCount; i++)
            {
                //Reverse the matrix and store global displacements in a joint vector. Convert defelction vector so it gives a matrix for the end member deflections
                //Going to be honest, not sure about this code block. Seems to work however so theres that. Need more checkign to be sure
                var jointDeflection = V.Dense(2 * 2);
                int startJointID = (int)MemberData[i, 0];
                int endJointID = (int)MemberData[i, 1];

                int j = startJointID * 2;
                for (int k = 0; k < 2; k++)
                {
                    var n = (int)StructureCoordinates[(int)j];
                    j++;
                    if (n < NDoF)
                    {
                        jointDeflection[k] = Displacements[n];
                    }
                }
                j = endJointID * 2;
                for (int k = 2; k < 4; k++)
                {
                    var n = (int)StructureCoordinates[(int)j];
                    j++;
                    if (n < NDoF)
                    {
                        jointDeflection[k] = Displacements[n];
                    }
                }

                //Determine transformation matrix for the member
                //Calculate the length using Pythagorean theorem
                double Length = Math.Pow(Math.Pow(JointCoordinates[startJointID, 0] - JointCoordinates[endJointID, 0], 2) + Math.Pow(JointCoordinates[startJointID, 1] - JointCoordinates[endJointID, 1], 2), 0.5);
                double cosTheta = (JointCoordinates[endJointID, 0] - JointCoordinates[startJointID, 0]) / Length;
                double sinTheta = (JointCoordinates[endJointID, 1] - JointCoordinates[startJointID, 1]) / Length;

                var TransformationMatrix = M.Dense(4, 4);
                TransformationMatrix[0, 0] = cosTheta;
                TransformationMatrix[0, 1] = sinTheta;
                TransformationMatrix[2, 2] = cosTheta;
                TransformationMatrix[2, 3] = sinTheta;
                TransformationMatrix[1, 0] = -sinTheta;
                TransformationMatrix[1, 1] = cosTheta;
                TransformationMatrix[3, 2] = -sinTheta;
                TransformationMatrix[3, 3] = cosTheta;

                var localDisplacement = TransformationMatrix.Multiply(jointDeflection);

                //Calculate the local stiffness matrix
                double EM = ElasticModulus[(int)MemberData[i, 2]];
                double Area = CrossSectionalArea[(int)MemberData[i, 3]];
                double Z = EM * Area / Length;

                var BK = M.Dense(4, 4);
                BK[0, 0] = Z;
                BK[0, 2] = -Z;
                BK[2, 0] = -Z;
                BK[2, 2] = Z;

                //Local end forces
                var Q = BK.Multiply(localDisplacement);

                //Global end forces
                var F = TransformationMatrix.Transpose().Multiply(Q); 
                
                //Store global end forces in reaction matrix
                for(int k = 0; k < 4; k++)
                {
                    int l1;
                    if(k<2)
                    {
                        l1 = startJointID * 2 + k;
                    }
                    else
                    {
                        //This right??
                        l1 = endJointID * 2 + (k - 2);                        
                    }

                    var n = (int)StructureCoordinates[l1];
                    if (n >= NDoF)
                    {
                        Reactions[n - NDoF] = Reactions[n - NDoF] + F[k];
                    }
                }
            }
        }

        public void PrintInput()
        {
            Console.WriteLine(JointCoordinates);
            Console.WriteLine(SupportData);
            Console.WriteLine(MemberData);
            Console.WriteLine(LoadData);
        }

        private int NumberOfRestraints()
        {
            int result = 0;

            for(int i = 0; i < SupportData.RowCount; i++)
            {
                if(SupportData[i,1] != 0)
                {
                    result++;
                }
                if (SupportData[i, 2] != 0)
                {
                    result++;
                }
            }

            return result;
        }

        private Vector<double> AssignStructureNumbers(int DegreesOfFreedom)
        {
            int JointCount = 0;
            int RestraintCount = DegreesOfFreedom;
            //int RestraintCount = DegreesOfFreedom;

            var result = V.Dense(JointCoordinates.RowCount * 2);

            for (int i = 0; i < JointCoordinates.RowCount; i++)
            {
                //Iterate through the supports to determine if current joint is one
                bool support = false;
                for (int j = 0; j < SupportData.RowCount; j++)
                {
                    if (SupportData[j, 0] == i)
                    {
                        support = true;
                        //Check for restraint in X axis
                        if (SupportData[j, 1] != 0)
                        {
                            result[i * 2] = RestraintCount;
                            RestraintCount++;
                        }
                        else
                        {
                            result[i * 2] = JointCount;
                            JointCount++;
                        }

                        //Check for restraint in Y axis
                        if (SupportData[j, 2] != 0)
                        {
                            result[i * 2 + 1] = RestraintCount;
                            RestraintCount++;
                        }
                        else
                        {
                            result[i * 2 + 1] = JointCount;
                            JointCount++;
                        }
                    }
                }
                if (!support)
                {
                    //Not a support, therefore label x and y axis
                    result[i * 2] = JointCount;
                    JointCount++;

                    result[i * 2 + 1] = JointCount;
                    JointCount++;
                }
            }

            return result;
        }

        private Matrix<double> BuildStiffnesMatrix(int i)
        {
            var result = M.Dense(4, 4);

            //Get section properties
            int startJointID = (int)MemberData[i, 0];
            int endJointID = (int)MemberData[i, 1];
            double EM = ElasticModulus[(int)MemberData[i, 2]];
            double Area = CrossSectionalArea[(int)MemberData[i, 3]];

            //Calculate the length using Pythagorean theorem
            double Length = Math.Pow(Math.Pow(JointCoordinates[startJointID, 0] - JointCoordinates[endJointID, 0], 2) + Math.Pow(JointCoordinates[startJointID, 1] - JointCoordinates[endJointID, 1], 2), 0.5);
            double cosTheta = (JointCoordinates[endJointID, 0] - JointCoordinates[startJointID, 0]) / Length;
            double sinTheta = (JointCoordinates[endJointID, 1] - JointCoordinates[startJointID, 1]) / Length;

            double Z, Z1, Z2, Z3;
            Z = EM * Area / Length;
            Z1 = Z * Math.Pow(cosTheta, 2);
            Z2 = Z * Math.Pow(sinTheta, 2);
            Z3 = Z * sinTheta * cosTheta;

            result[0, 0] = Z1;
            result[1, 0] = Z3;
            result[2, 0] = -Z1;
            result[3, 0] = -Z3;
            result[0, 1] = Z3;
            result[1, 1] = Z2;
            result[2, 1] = -Z3;
            result[3, 1] = -Z2;
            result[0, 2] = -Z1;
            result[1, 2] = -Z3;
            result[2, 2] = Z1;
            result[3, 2] = Z3;
            result[0, 3] = -Z3;
            result[1, 3] = -Z2;
            result[2, 3] = Z3;
            result[3, 3] = Z2;

            return result;        
        }
    }
}
