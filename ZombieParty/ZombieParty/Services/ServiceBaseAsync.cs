using Microsoft.EntityFrameworkCore;
using ZombieParty.Models.Data;

namespace ZombieParty.Services
{
    public class ServiceBaseAsync<T> : IServiceBaseAsync<T> where T: class
    {
        /* La classe est aussi générique. T sera remplacé par le vrai type lors de l'utilisation. 
         * La classe implémente l'interface IServiceBaseAsync<T>. where T : class est une contrainte de type, il faut donc que T soit une classe. */
        protected readonly ZombiePartyDbContext _dbContext;

        public ServiceBaseAsync(ZombiePartyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> CreateAsync(T entity)
        {
            /* Set<T>() est une methode qui retourne le DbSet correspondant sans avoir à le nommer explicitement. Par exemple, si le type T est Zombie, 
             * Set<T>() retournera le DbSet Zombies. Le résultat sera : await _dbContext.Zombies.AddAsync(entity); */
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;

        }

        public virtual async Task DeleteAsync(int id) // virtual, car nous allons override plus tard
        {
            /* On appelle d'abord GetByIdAsync (méthode présentée au bas de la page) pour vérifier que l'entité existe. Si elle n'existe pas (null), rien ne se passe. Sinon l’entité est supprimée */
            var entity = await this.GetByIdAsync(id);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task EditAsync(T entity)
        {

            /* Entity Framework peut "tracker" (suivre) les entités qu'il a chargées depuis la BD et il garde en mémoire des informations sur l'état (state) de ces objets. 
             * Entry(entity).State permet d’accéder à cet état. Quand l’objet provient d'un formulaire HTML via une requête POST par exemple, 
             * il N'EST PAS tracké par EF. Son état (state) est alors Detached (EntityState.Detached). 
             * Dans ce cas-ci, (if est true) on voudra faire le update pour que l’objet soit "tracker" par EF et on mettra à jour la BD. Si l’objet est déjà "tracker", 
             * on spécifiera que l’état est Modified, pour être certain SaveChangesAsync() fasse bien un update de l’objet. */
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbContext.Update<T>(entity);
            }
            else
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();

        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            /* Retourne une liste d’entités du type correspondant. */
            return await _dbContext.Set<T>().ToListAsync();

        }

        public async Task<T?> GetByIdAsync(int id)
        {
            /* Cherche l’entité du type correspondant. Le type de retour T? signifie que la méthode peut retourner null si l'entité n'existe pas. */
            return await _dbContext.Set<T>().FindAsync(id);

        }
    }
}
