using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
