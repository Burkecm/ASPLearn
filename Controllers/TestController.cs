using LeaveManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View(new TestViewModel() { Name = "Chad", DOB = new DateOnly(1991, 05, 20)});
        }
    }
}
