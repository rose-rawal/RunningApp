using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _context;
        public ClubRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            var data=await _context.Clubs.Where(c=>c.Address.City== city).ToListAsync();
            return data;
        }

        public async Task<Club> GetClubById(int id)
        {
            var data=await _context.Clubs.Include(c=>c.Address).FirstOrDefaultAsync(c=>c.Id== id);
            return data;
        }
        public async Task<Club> GetClubByIdNoTracking(int id)
        {
            var data = await _context.Clubs.Include(c => c.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            return data;
        }

        public async Task<ICollection<Club>> GetClubs()
        {
            var data=await _context.Clubs.ToListAsync();
            return data;
        }

        public bool Save()
        {
            var data=_context.SaveChanges();
            return data>0 ? true : false;   
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
