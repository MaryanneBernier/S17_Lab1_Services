using ZombieParty.Models;

namespace ZombieParty.Services
{
        public interface IZombieService : IServiceBaseAsync<Zombie>
        {
            Task<IEnumerable<Zombie>> GetAllByZombieTypeAsync(int zombieTypeId);
            Task<IReadOnlyList<Zombie>> GetAllIndexAsync();
            bool ZombieNameExist(string name);
    }
    }

