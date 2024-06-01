using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAll();
        Task<Race> GetRaceById(int id);
        Task<IEnumerable<Race>> GetRaceByCity(string city);
        bool Add(Race race);    
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();

    }
}
