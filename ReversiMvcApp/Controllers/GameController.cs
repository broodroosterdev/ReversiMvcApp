using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Data;
using ReversiMvcApp.Helpers;
using ReversiMvcApp.Services;

namespace ReversiMvcApp.Controllers
{
    [ApiController]
    [Route("Game")]
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private readonly ReversiDbContext _context;
        private readonly IApiService _apiService;

        public GameController(ILogger<GameController> logger, ReversiDbContext context, IApiService apiService)
        {
            _context = context;
            _logger = logger;
            _apiService = apiService;
        }

        // GET Game/New
        [Authorize]
        [HttpGet("New")]
        public IActionResult New()
        {
            return View();
        }

        [Authorize]
        [HttpPost("New")]
        public async Task<IActionResult> NewPost([FromForm] string description)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _apiService.CreateNew(currentUserID, description);
            if (result.Failure)
            {
                Console.WriteLine(result.Error);
                return Redirect($"/Home/Error");
            }

            return Redirect($"/Home/Index/{result.Value}");
        }

        [Authorize]
        [HttpGet("Current/{gameToken}")]
        public async Task<IActionResult> Current([FromRoute] string gameToken)
        {
            var game = await _apiService.GetGame(gameToken);
            if (game.Failure)
                return Redirect("/Home/Error");

            ViewData["Game"] = game.Value;
            return View();
        }
    }
}