using DLP.UI;
//using DLP.UI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DLP.UI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public ViewResult Index()
        {
            // Arrange
            HomeControllerTest controller = new HomeControllerTest();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            return result;
        }

        [TestMethod]
        public ViewResult About()
        {
            // Arrange
            HomeControllerTest controller = new HomeControllerTest();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
            return result;
        }

        [TestMethod]
        public ViewResult Contact()
        {
            // Arrange
            HomeControllerTest controller = new HomeControllerTest();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            return result;
        }
    }
}
