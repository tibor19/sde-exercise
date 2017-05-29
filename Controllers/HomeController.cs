using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreOAuth2Sample.ViewModel;
using AspNetCoreOAuth2Sample.Services;

namespace AspNetCoreOAuth2Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiService _apiService;
        public HomeController(IApiService apiService)
        {
            _apiService = apiService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> Rules()
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
