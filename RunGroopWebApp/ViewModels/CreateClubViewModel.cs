using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroopWebApp.ViewModels
{
    public class CreateClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public ClubCategory ClubCategory { get; set; }
    }
}
