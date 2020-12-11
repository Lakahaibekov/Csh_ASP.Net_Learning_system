using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using CourseProject.Controllers;
using CourseProject.Models;
//using CourseProject.Rep;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nest;

namespace CourceProject.Tests.Controllers
{
    [TestClass]
    public class StudentsControllerTest
    {
        private StudentsController controller;
        private ViewResult result;

        [TestInitialize]
        public void SetupContext()
        {
            controller = new StudentsController();
            result = controller.Index() as ViewResult;
        }

        [TestMethod]
        public void IndexViewResultNotNull()//if remove 'Include(*) from controllers will work
        {
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void IndexViewEqualIndexCshtml()
        {
            Assert.AreEqual("Index", result.ViewName);
        }

        //from metanit using moq
        /*
        [TestMethod]
        public void IndexViewModelNotNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetSchoolKidList()).Returns(new List<SchoolKid>());
            SchoolKidsController controller = new SchoolKidsController(mock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }*/
    }
}
