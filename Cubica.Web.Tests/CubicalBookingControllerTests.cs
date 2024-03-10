using Cubica.Core.IServices;
using Cubica.Models.Model;
using Cubica.Models.ViewModel;
using Cubica.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubica.Web.Tests
{
    [TestFixture]
    public class CubicalBookingControllerTests
    {
        private Mock<ICubicalBookingService> _cubicalBookingService;
        private CubicalBookingController _bookingController;


        [SetUp]
        public void Setup()
        {
            _cubicalBookingService = new Mock<ICubicalBookingService>();
            _bookingController = new CubicalBookingController(_cubicalBookingService.Object);
        }

        [Test]
        public void IndexPage_CallRequest_VerifyGellAllInvoked()
        {
            _bookingController.Index();
            _cubicalBookingService.Verify(x => x.GetAllBooking(), Times.Once());
        }

        [Test]
        public async Task BookCubical_ModelStateInvalid_ReturnView()
        {
            _bookingController.ModelState.AddModelError("test", "test");

            var result = await _bookingController.Book(new CubicalBooking() { Email = "", FirstName = "", LastName = "" });

            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual("Book", viewResult.ViewName);
        }

        [Test]
        public async Task BookCubicalCheck_NotSuccessful_NoCubicalCode()
        {
            _cubicalBookingService.Setup(x => x.BookCubical(It.IsAny<CubicalBooking>()))
                .ReturnsAsync(new CubicalBookingResult()
                {
                    Email = "",
                    FirstName = "",
                    LastName = "",
                    Code = CubicalBookingCode.NoCubicalAvailable,
                });

            var result = await _bookingController.Book(new CubicalBooking() { Email = "", FirstName = "", LastName = "" });
            Assert.IsInstanceOf<ViewResult>(result);
            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual("No cubical available for selected date", viewResult.ViewData["Error"]);
        }

        [Test]
        public async Task BookCubicalCheck_Successful_SuccessCodeAndRedirect()
        {
            //arrange
            _cubicalBookingService.Setup(x => x.BookCubical(It.IsAny<CubicalBooking>()))
                .ReturnsAsync((CubicalBooking booking) => new CubicalBookingResult()
                {
                    Email = booking.Email,
                    FirstName = booking.FirstName,
                    LastName = booking.LastName,
                    Code = CubicalBookingCode.Success,
                });

            //act
            var result = await _bookingController.Book(new CubicalBooking()
            {
                FirstName = "hello",
                LastName = "mello",
                Email = "hellomello@gmail.com",
                Date = DateTime.Now,
                CubicalId = 1,
            });

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult actionResult = result as RedirectToActionResult;
            Assert.AreEqual("hello", actionResult.RouteValues["FirstName"]);
            Assert.AreEqual(CubicalBookingCode.Success, actionResult.RouteValues["Code"]);
        }
    }
}
