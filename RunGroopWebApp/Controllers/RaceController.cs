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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data=_context.GetRaceById(id);
            if(data == null)
                return View("Not Found");
            var dataStr = new CreateRaceViewModel
            {
                Title = data.Result.Title,
                Description = data.Result.Description,
                AddressId = data.Result.AddressId,
                Address = data.Result.Address,
                RaceCategory = data.Result.RaceCategory,
            };
            return View(dataStr);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateRaceViewModel data)
        {
            if (!ModelState.IsValid)
                return View("Error posting");
            var oldData = await _context.GetRaceById(id);
            try
            {
                _photoService.DeletePhotoAsync(oldData.Image);
                var newImage = await _photoService.AddPhotoAsync(data.Image);
                oldData.Image = newImage.Url.ToString();
                oldData.RaceCategory = data.RaceCategory;
                oldData.Title = data.Title;
                oldData.Description = data.Description;
                oldData.AddressId = data.AddressId;
                oldData.Address = data.Address;
                bool isSaved = _context.Save();
                if (isSaved)
                    return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error Deleting Image");
                return View(data);
            }
            return View(data);
        }
    }
}
