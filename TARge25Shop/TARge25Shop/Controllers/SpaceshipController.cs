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
    private readonly IFileServices _fileServices;

    public SpaceshipController(
        ISpaceshipServiceInterface spaceshipService, TARge25ShopContext dbContext, IFileServices fileServices)
    {
        _spaceshipService = spaceshipService;
        _dbContext = dbContext;
        _fileServices = fileServices;
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
        SpaceshipUpdateViewmodel result = new();
        return View("Create", result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(SpaceshipCreateViewModel viewmodel)
    {
        var dto = new SpaceshipDto
        {
            Name = viewmodel.Name,
            ShipType = viewmodel.ShipType,
            MaxCrewSize = viewmodel.MaxCrewSize,
            EnginePower = viewmodel.EnginePower,
            Files = viewmodel.Files,
            // FileToApiDtos = viewmodel.Images
            //     .Select(x => new FileToApiDto
            //     {
            //         Id = x.ImageId,
            //         ExistingFilePath = x.FilePath,
            //         SpaceshipId = x.SpaceshipId
            //     })
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

        var images = await GetImagesBySpaceshipId(id);
        
        var viewmodel = new SpaceshipUpdateViewmodel
        {
            Id = spaceship.Id,
            Name = spaceship.Name,
            ShipType = spaceship.ShipType,
            MaxCrewSize = spaceship.MaxCrewSize,
            EnginePower = spaceship.EnginePower,
            CreatedAt = spaceship.CreatedAt,
            UpdatedAt = spaceship.UpdatedAt
        };
        
        viewmodel.Images.AddRange(images);
        
        return View("Update", viewmodel);
    }
        
    [HttpPost]
    public async Task<IActionResult> Update(SpaceshipUpdateViewmodel viewmodel)
    {
        if (!ModelState.IsValid)
        {
            return View("Update", viewmodel);
        }

        var dto = new SpaceshipDto
        {
            Id = viewmodel.Id,
            Name = viewmodel.Name,
            ShipType = viewmodel.ShipType,
            MaxCrewSize = viewmodel.MaxCrewSize,
            EnginePower = viewmodel.EnginePower,
            CreatedAt = viewmodel.CreatedAt,
            UpdatedAt = viewmodel.UpdatedAt,
            Files = viewmodel.Files,
            FileToApiDtos = MapToImageDtos(viewmodel)
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

        var images = await GetImagesBySpaceshipId(id);
        
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
        
        viewModel.Images.AddRange(images);

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

        var images = await GetImagesBySpaceshipId(id);
        
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
        
        viewmodel.Images.AddRange(images);

        return View(viewmodel);
    }
    
    [HttpPost]
    public async Task<IActionResult> RemoveImage(ImageViewModel vm)
    {
        var dto = new FileToApiDto()
        {
            Id = vm.ImageId
        };

        var image = await _fileServices.RemoveImageFromApi(dto);

        if (image == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<ImageViewModel[]> GetImagesBySpaceshipId(Guid id)
    {
        var images = await _dbContext.FilesToApis
            .Where(x => x.ObjectId == id)
            .Select(y => new ImageViewModel
            {
                FilePath = y.ExistingFilePath,
                ImageId = y.Id
            }).ToArrayAsync<ImageViewModel>();
        
        return images;
    }

    private FileToApiDto[] MapToImageDtos(SpaceshipUpdateViewmodel viewmodel)
    {
        return viewmodel.Images
            .Select(x => new FileToApiDto
            {
                Id = x.ImageId,
                ExistingFilePath = x.FilePath,
                SpaceshipId = x.SpaceshipId
            }).ToArray();
    }
}