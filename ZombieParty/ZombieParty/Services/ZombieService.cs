using Microsoft.EntityFrameworkCore;
using ZombieParty.Models;
using ZombieParty.Models.Data;

namespace ZombieParty.Services

{
    public class ZombieService : ServiceBaseAsync<Zombie>, IZombieService
    {
        public ZombieService(ZombiePartyDbContext dbContext): base(dbContext)
        {
            
        }


        public async Task<IEnumerable<Zombie>> GetAllByZombieTypeAsync(int zombieTypeId)
        {
            return await _dbContext.Zombies
                .Where(z => z.ZombieTypeId == zombieTypeId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Zombie>> GetAllIndexAsync()
        {
            return await _dbContext.Set<Zombie>().OrderBy(z => z.Name).Include(z => z.ZombieType).ToListAsync();
        }

        public bool ZombieNameExist(string name)
        {
            var ZombieSameName = _dbContext.Zombies.Any(x => x.Name == name);
            return ZombieSameName;
        }


    }
}
