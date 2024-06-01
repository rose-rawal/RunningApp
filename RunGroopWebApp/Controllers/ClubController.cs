using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClubController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Club> data=_context.Clubs.ToList();
            return View(data);
        }
        public IActionResult Details(int id) { 
            Club club = _context.Clubs.Include(n=>n.Address).FirstOrDefault(c => c.Id == id);
            return View(club);
        }
    }
}
