using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MHalas.WSI.Lab1.Rest.Controllers;

namespace MHalas.WSI.Lab1.Rest.Tests.Controllers
{
    [TestClass]
    public class GradeControllerTest
    {
        GradesController controller;

        [TestInitialize]
        public void TestInit()
        {
            controller = new GradesController();
        }

        [TestMethod]
        public void GetGrades()
        {
        }
    }
}
