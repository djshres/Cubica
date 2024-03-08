using Cubica.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cubica.Web.Controllers
{
    public class CubicalController : Controller
    {
        private readonly ICubicalService _cubicalService;

        public CubicalController(ICubicalService cubicalService)
        {
            _cubicalService = cubicalService;
        }

        public async Task<IActionResult> Index()
        {
            var cubicles = await _cubicalService.GetAll();
            return View(cubicles);
        }
    }
}
