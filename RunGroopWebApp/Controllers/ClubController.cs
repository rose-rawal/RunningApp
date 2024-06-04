using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data=await _context.GetClubById(id);
            
            if(data == null)
                return View(null);
            var dataSend = new CreateClubViewModel
            {
                Title = data.Title,
                Description = data.Description,
                Address = data.Address,
                
                AddressId = data.AddressId,
                ClubCategory = data.ClubCategory,
            };
            return View(dataSend);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,CreateClubViewModel club)
        {
            Console.WriteLine($"id:{id}// club:{club.Address}");

            if (!ModelState.IsValid)
            {
                
                ModelState.AddModelError("", "Error in finding the data");
                Console.WriteLine($"error1:{ModelState}");
                return View(club);
            }
            var oldData=await _context.GetClubByIdNoTracking(id);
            if(oldData != null)
            {
                Console.WriteLine("data found");
                try
                {
                    _photoService.DeletePhotoAsync(oldData.Image);
                } catch (Exception ex) {
                    ModelState.AddModelError("", "Error Replacing Image");
                    Console.WriteLine($"error2:{ModelState}");
                    return View(oldData);
                }
                var newImage=await _photoService.AddPhotoAsync(club.Image);
                if (newImage != null) {
                    var newData = new Club
                    {
                        Id = id,
                        Title = club.Title,
                        Description = club.Description,
                        Image = newImage.Url.ToString(),
                        AddressId = club.AddressId,
                        Address = club.Address,
                    };
                    _context.Update(newData);
                    Console.WriteLine($"No Error:{ModelState}");
                    return RedirectToAction("Index");
                }
            }
            return View(club);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        { 
            var clubData=await _context.GetClubById(id);
            return View(clubData);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
     
            var deleteItem=await _context.GetClubById(id);
            Console.WriteLine($"deleteItem:{deleteItem}");
            if (deleteItem == null)
                return View();
            try
            {
                _context.Delete(deleteItem);
                Console.WriteLine($"data delete{deleteItem}");
            }
            catch (Exception ex) { 
                Console.WriteLine($"errormsg:{ex.Message}" );
                return View();
            }
            return RedirectToAction("Index");
        }
        
    }
}
