using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces
{
    public interface IClubRepository
    {
        Task<ICollection<Club>> GetClubs();
        Task<Club> GetClubById(int id);
        Task<Club> GetClubByIdNoTracking(int id);
        Task<IEnumerable<Club>> GetClubByCity(string city);
        bool Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);
        bool Save();

    }
}
