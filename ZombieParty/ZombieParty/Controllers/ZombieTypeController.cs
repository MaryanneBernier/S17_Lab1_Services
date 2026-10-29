using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZombieParty.Models;
using ZombieParty.Models.Data;
using ZombieParty.Services;
using ZombieParty.Utility;
using ZombieParty.ViewModels;

namespace ZombieParty.Controllers
{
    public class ZombieTypeController : Controller
    {
        //private ZombiePartyDbContext _baseDonnees { get; set; }

        private IZombieTypeService _serviceZT { get; set; }

        private IZombieService _serviceZ { get; set; }

        public ZombieTypeController(IZombieTypeService serviceZT, IZombieService serviceZ)
        {
            //_baseDonnees = baseDonnees;

            _serviceZT = serviceZT;

            _serviceZ = serviceZ;
        }

        public async Task<IActionResult> Index()
        {
            //List<ZombieType> zombieTypesList = await _baseDonnees.ZombieTypes.ToListAsync();

            //return View(zombieTypesList);

            return View(await _serviceZT.GetAllAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var zombies = (await _serviceZ.GetAllByZombieTypeAsync(id)).ToList();
            ZombieTypeVM zombieTypeVM = new()
            {
                ZombieType = new(),
                ZombiesList = zombies,
                ZombiesCount = zombies.Count(),
                PointsAverage = zombies.Any() ? zombies.Average(p => p.Point) : 0
            };

            var zombieType = await _serviceZT.GetByIdAsync(id);

            if (zombieType == null)
            {
                return NotFound();
            }

            return View(zombieTypeVM);
        }


        //GET CREATE
        public IActionResult Create()
        {
            //ici pas grand intérêt à la mettre async
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZombieType zombieType)
        {
            if (ModelState.IsValid)
            {
                // Ajouter à la BD
                await _serviceZT.CreateAsync(zombieType);
                TempData[AppConstants.Success] = $"{zombieType.TypeName} zombie type added";
                return this.RedirectToAction("Index");
            }

            return this.View(zombieType);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ZombieType? zombieType = await _serviceZT.GetByIdAsync(id);

            return View(zombieType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ZombieType zombieType)
        {
            if (ModelState.IsValid)
            {
                await _serviceZT.EditAsync(zombieType);
                TempData[AppConstants.Success] = $"ZombieType {zombieType.TypeName} has been modified";
                return this.RedirectToAction("Index");
            }

            return View(zombieType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            ZombieType? zombieType = await _serviceZT.GetByIdAsync(id);
            if (zombieType == null)
            {
                return NotFound();
            }

            return View(zombieType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            ZombieType? zombieType = await _serviceZT.GetByIdAsync(id);
            if (zombieType == null)
            {
                return NotFound();
            }

            await _serviceZT.DeleteAsync(id);
            TempData[AppConstants.Success] = $"ZombieType {zombieType.TypeName} has been removed";
            return RedirectToAction("Index");
        }
    }
}
