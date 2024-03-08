using Cubica.Core.IServices;
using Cubica.Models.Model;
using Cubica.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Cubica.Web.Controllers
{
    public class CubicalBookingController : Controller
    {
        private readonly ICubicalBookingService _cubicalBookingService;

        public CubicalBookingController(ICubicalBookingService cubicalBookingService)
        {
            _cubicalBookingService = cubicalBookingService;
        }

        public async Task<IActionResult> Index()
        {
            var allBookings =await _cubicalBookingService.GetAllBooking();
            return View(allBookings);
        }

        public IActionResult Book()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Book(CubicalBooking cubicalBooking)
        {
            IActionResult actionResult = View("Book");
            if (ModelState.IsValid)
            {
                var result =await _cubicalBookingService.BookCubical(cubicalBooking);
                if (result.Code == CubicalBookingCode.Success)
                {
                    actionResult = RedirectToAction("BookingConfirmation", result);
                }
                else if (result.Code == CubicalBookingCode.NoCubicalAvailable)
                {
                    ViewData["Error"] = "No cubical available for selected date";
                }
            }

            return actionResult;
        }

        public IActionResult BookingConfirmation(CubicalBookingResult cubicalBookingResult)
        {
            return View(cubicalBookingResult);
        }
    }
}
