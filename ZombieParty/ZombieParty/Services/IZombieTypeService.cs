using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZombieParty.Models;

namespace ZombieParty.Services
{
    public interface IZombieTypeService : IServiceBaseAsync<ZombieType>
    {
      bool HasAssociatedZombies(int id);
        IEnumerable<SelectListItem>? ListZombieTypeDisponible();
    }


}
