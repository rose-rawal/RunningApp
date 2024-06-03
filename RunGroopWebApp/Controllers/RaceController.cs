using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _context;
        private readonly IPhotoService _photoService;
        public RaceController(IRaceRepository context,IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _context.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Details(int id)
        {
            Race race = await _context.GetRaceById(id);
            return View(race);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel race)
        {
            if(ModelState.IsValid)
            {
                var imageData=await _photoService.AddPhotoAsync(race.Image);
                var raceData = new Race
                {
                    Title = race.Title,
                    Description = race.Description,
                    Image = imageData.Url.ToString(),
                    Address = race.Address,
                };
                _context.Add(raceData);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error uploading image");
            }
            return View(race);
                
           
        }
    }
}
