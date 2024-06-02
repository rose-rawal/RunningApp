using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _context;
        public readonly IPhotoService _photoService;
        public ClubController(IClubRepository context,IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
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
        public async Task<IActionResult> Create(CreateClubViewModel club)
        {
            if (ModelState.IsValid) {
                var result = await _photoService.AddPhotoAsync(club.Image);
                
                var clubData = new Club
                {
                    Title = club.Title,
                    Description = club.Description,
                    Image = result.Url.ToString(),
                    Address=new Address
                    {
                        City=club.Address.City,
                        State=club.Address.State,
                        Street=club.Address.Street
                    }
                };
                _context.Add(clubData);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload Failed");
            }
            return View(club);
        }
        
    }
}
