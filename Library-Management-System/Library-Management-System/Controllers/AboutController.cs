using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}