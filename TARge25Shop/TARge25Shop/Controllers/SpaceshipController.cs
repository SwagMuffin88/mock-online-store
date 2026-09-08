using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core;
using TARge25Shop.Models.Spaceship;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Controllers;

public class SpaceshipController : Controller
{
    private readonly ISpaceshipServiceInterface _spaceshipService;

    public SpaceshipController(ISpaceshipServiceInterface spaceshipService)
    {
        _spaceshipService = spaceshipService;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult CreateIndex()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewSpaceship(SpaceshipCreateViewModel viewmodel)
    {
        var dto = new SpaceshipDto
        {
            Name = viewmodel.Name,
            ShipType = viewmodel.ShipType,
            MaxCrewSize = viewmodel.MaxCrewSize,
            EnginePower = viewmodel.EnginePower
        };

        var spaceship = await _spaceshipService.Create(dto);
        return RedirectToAction("Index", "Spaceship", new { id = spaceship.Id });
    }
    
}