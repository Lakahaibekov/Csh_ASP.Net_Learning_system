using System.Web.Mvc;
using CourseProject.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CourseProject.Tests.Controllers
{
    [TestClass]
    public class CoursesControllerTest
    {
        private CoursesController controller;
        private ViewResult result;

        [TestInitialize]
        public void SetupContext()
        {
            controller = new CoursesController();
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
