using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ReversiMvcApp.Helpers;
using ReversiMvcApp.Services;

namespace ReversiMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ReversiDbContext _context;
        private readonly IApiService _apiService;

        public HomeController(ILogger<HomeController> logger, ReversiDbContext context, IApiService apiService)
        {
            _context = context;
            _logger = logger;
            _apiService = apiService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            if(await _context.Players.FindAsync(currentUserID) == null)
            {
                var player = new Player
                {
                    Guid = currentUserID,
                };
                await _context.Players.AddAsync(player);
                await _context.SaveChangesAsync();
            }

            var descriptions = await _apiService.GetPendingGames();
            if (descriptions.Failure)
            {
                Console.WriteLine(descriptions.Error);
                return Error();
            }
            ViewData["Descriptions"] = descriptions.Value;
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
