using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core;
using TARge25Shop.Models.Spaceship;
using TARge25Shop.Core.Dto;
using TARge25Shop.Data;

namespace TARge25Shop.Controllers;

public class SpaceshipController : Controller
{
    private readonly ISpaceshipServiceInterface _spaceshipService;
    private readonly TARge25ShopContext _dbContext;

    public SpaceshipController(ISpaceshipServiceInterface spaceshipService, TARge25ShopContext dbContext)
    {
        _spaceshipService = spaceshipService;
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = _dbContext.Spaceships
            .Select(s => new SpaceshipIndexViewModel 
            {
                Id = s.Id,
                Name = s.Name,
                ShipType = s.ShipType,
                MaxCrewSize = s.MaxCrewSize,
                EnginePower = s.EnginePower,
                UpdatedAt = s.UpdatedAt,
                CreatedAt =   s.CreatedAt
            });
        
        return View("Index", await result.ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(SpaceshipCreateViewModel viewmodel)
    {
        // if (!ModelState.IsValid)
        // {
        //     return View(viewmodel);
        // }
        
        var dto = new SpaceshipDto
        {
            // Id = viewmodel.Id,
            Name = viewmodel.Name,
            ShipType = viewmodel.ShipType,
            MaxCrewSize = viewmodel.MaxCrewSize,
            EnginePower = viewmodel.EnginePower,
            // CreatedAt = viewmodel.CreatedAt
        };
        
        var result = await _spaceshipService.Create(dto);

        if (result == null)
        {
            ModelState.AddModelError(
                string.Empty, "Could not create spaceship! Check your fields and try again."
            );
        }
        
        return RedirectToAction("Index", "Spaceship", new { id = result.Id });
    }
    
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var spaceship = await _spaceshipService.DetailAsync(id);
        
        if (spaceship == null) 
        {
            return NotFound();
        } 
        
        var viewmodel = new SpaceshipUpdateViewModel
        {
            Id = spaceship.Id,
            Name = spaceship.Name,
            ShipType = spaceship.ShipType,
            MaxCrewSize = spaceship.MaxCrewSize,
            EnginePower = spaceship.EnginePower,
            CreatedAt = spaceship.CreatedAt,
            UpdatedAt = spaceship.UpdatedAt
        };
        
        return View(viewmodel);
    }
        
    [HttpPost]
    public async Task<IActionResult> Update(SpaceshipUpdateViewModel viewmodel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewmodel);
        }

        var dto = new SpaceshipDto
        {
            Id = viewmodel.Id,
            Name = viewmodel.Name,
            ShipType = viewmodel.ShipType,
            MaxCrewSize = viewmodel.MaxCrewSize,
            EnginePower = viewmodel.EnginePower,
            CreatedAt = viewmodel.CreatedAt,
            UpdatedAt = viewmodel.UpdatedAt
        };
        var result = await _spaceshipService.Update(dto); 

        if (result == null)
        {
            return NotFound();
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var spaceship = await _spaceshipService.DetailAsync(id);

        if (spaceship == null)
        {
            return NotFound();
        }

        var viewModel = new SpaceshipDeleteViewModel
        {
            Id = spaceship.Id,
            Name = spaceship.Name,
            ShipType = spaceship.ShipType,
            MaxCrewSize = spaceship.MaxCrewSize,
            EnginePower = spaceship.EnginePower,
            CreatedAt = spaceship.CreatedAt,
            UpdatedAt = spaceship.UpdatedAt
        };

        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmation(Guid id)
    {
        var spaceship = await _spaceshipService.Delete(id);
        
        if (spaceship == null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var spaceship = await _spaceshipService.DetailAsync(id);

        if (spaceship == null)
        {
            return NotFound();
        }
        
        var viewmodel = new SpaceshipDetailsViewModel
        {
            Id = spaceship.Id,
            Name = spaceship.Name,
            ShipType = spaceship.ShipType,
            MaxCrewSize = spaceship.MaxCrewSize,
            EnginePower = spaceship.EnginePower,
            CreatedAt = spaceship.CreatedAt,
            UpdatedAt = spaceship.UpdatedAt
        };

        return View(viewmodel);
    }
    
}