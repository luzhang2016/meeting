using Microsoft.AspNetCore.Mvc;

namespace Meeting.Admin.Controllers
{
    /// <summary>
    /// Home控制器
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        
        /// <summary>
        /// 错误
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View();
        }

    }
}
