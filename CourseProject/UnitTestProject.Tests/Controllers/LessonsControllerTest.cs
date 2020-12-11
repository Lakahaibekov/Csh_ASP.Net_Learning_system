using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using CourseProject.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CourceProject.Tests.Controllers
{
    [TestClass]
    public class LessonsControllerTest
    {
        private LessonsController controller;
        private ViewResult result;

        [TestInitialize]
        public void SetupContext()
        {
            controller = new LessonsController();
            result = controller.Index() as ViewResult;
        }

        [TestMethod]
        public void IndexViewResultNotNull()
        {
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void IndexViewEqualIndexCshtml()
        {
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
