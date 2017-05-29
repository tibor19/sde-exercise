using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreOAuth2Sample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Rules()
        {
            var vm = new RulesViewModel()
            {
                Clients = await _apiService.GetAllClients(),
                Rules = await _apiService.GetAllRules()
            };

            return View(vm);
        }
    }
}
