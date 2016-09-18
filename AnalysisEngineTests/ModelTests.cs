using Microsoft.VisualStudio.TestTools.UnitTesting;
using Venatu.SCL.AnalysisEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace Venatu.SCL.AnalysisEngine.Tests
{
    [TestClass()]
    public class ModelTests
    {
        [TestMethod()]
        public void Example3point8()
        {
            Model m = new Model(4, 3, 1, 2, 3, 1);

            m.SetJointCoordinate(0, 12 * 12, 16 * 12);
            m.SetJointCoordinate(1, 0, 0);
            m.SetJointCoordinate(2, 12 * 12, 0);
            m.SetJointCoordinate(3, 24 * 12, 0);

            m.SetSupport(0, 1, 1, 1);
            m.SetSupport(1, 2, 1, 1);
            m.SetSupport(2, 3, 1, 1);

            m.SetMaterial(0, 29000);

            m.SetSection(0, 8);
            m.SetSection(1, 6);

            m.SetMember(0, 1, 0, 0, 0);
            m.SetMember(1, 2, 0, 0, 1);
            m.SetMember(2, 3, 0, 0, 0);

            m.SetLoad(0, 0, 150, -300);

            m.Calculate();            

            Assert.AreEqual(Math.Round(m.Displacements[0],5), 0.21552);
            Assert.AreEqual(Math.Round(m.Displacements[1],5), -0.13995);
        }
    }
}