using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _context;
        public ClubController(IClubRepository context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<Club> data=await _context.GetClubs();
            return View(data);
        }
        public async Task<IActionResult> Details(int id) { 
            Club club = await _context.GetClubById(id);
            return View(club);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Club club)
        {
            if (!ModelState.IsValid) {
                return View(club);
            }
            _context.Add(club);
            return RedirectToAction("Index");      
        }
    }
}
