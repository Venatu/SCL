using Microsoft.VisualStudio.TestTools.UnitTesting;
using Venatu.SCL.AnalysisEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL.AnalysisEngine.Tests
{
    [TestClass()]
    public class ModelTests
    {
        [TestMethod()]
        public void ModelTest()
        {
            Model m = new Model(6, 3, 2, 3, 10, 3);
            Assert.Fail();
        }
    }
}