using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core;
using TARge25Shop.Core.Dto;
using TARge25Shop.Data;
using TARge25Shop.Models.Kindergarten;

namespace TARge25Shop.Controllers;

public class KindergartenController : Controller
{
    private readonly IKindergartenServiceInterface _kindergartenService;
    private readonly TARge25ShopContext _dbContext;
    public KindergartenController(IKindergartenServiceInterface kindergartenService, TARge25ShopContext dbContext)
    {
        _kindergartenService = kindergartenService;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = _dbContext.Kindergartens
            .Select(k => new KindergartenIndexViewModel
            {
                Id = k.Id,
                GroupName = k.GroupName,
                ChildrenCount = k.ChildrenCount,
                KindergartenName = k.KindergartenName,
                TeacherName = k.TeacherName,
                CreatedAt = k.CreatedAt,
                UpdatedAt = k.UpdatedAt
            });

        return View("Index", await result.ToListAsync());
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(KindergartenCreateViewModel viewmodel)
    {
        var dto = new KindergartenDto
        {
            GroupName = viewmodel.GroupName,
            ChildrenCount = viewmodel.ChildrenCount,
            KindergartenName = viewmodel.KindergartenName,
            TeacherName = viewmodel.TeacherName,
        };

        var result = await _kindergartenService.Create(dto);
        
        if (result == null)
        {
            ModelState.AddModelError(
                string.Empty, "Could not create kindergarten! Check your fields and try again."
            );
        }

        return RedirectToAction("Index", "Kindergarten", new { id = result.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var kindergarten = await _kindergartenService.DetailAsync(id);

        if (kindergarten == null)
        {
            return NotFound();
        }

        var viewmodel = new KindergartenUpdateViewmodel
        {
            Id = kindergarten.Id,
            GroupName = kindergarten.GroupName,
            ChildrenCount = kindergarten.ChildrenCount,
            KindergartenName = kindergarten.KindergartenName,
            TeacherName = kindergarten.TeacherName,
            CreatedAt = kindergarten.CreatedAt,
            UpdatedAt = kindergarten.UpdatedAt
        };

        return View(viewmodel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(KindergartenUpdateViewmodel viewmodel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewmodel);
        }

        var dto = new KindergartenDto
        {
            Id = viewmodel.Id,
            GroupName = viewmodel.GroupName,
            ChildrenCount = viewmodel.ChildrenCount,
            KindergartenName = viewmodel.KindergartenName,
            TeacherName = viewmodel.TeacherName,
            CreatedAt = viewmodel.CreatedAt,
            UpdatedAt = viewmodel.UpdatedAt
        };

        var result = await _kindergartenService.Update(dto);

        if (result == null)
        {
            return NotFound();
        } 
        
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var kindergarten = await _kindergartenService.DetailAsync(id);

        if (kindergarten == null)
        {
            return NotFound();
        }

        var viewmodel = new KindergartenDeleteViewmodel
        {
            Id = kindergarten.Id,
            GroupName = kindergarten.GroupName,
            ChildrenCount = kindergarten.ChildrenCount,
            KindergartenName = kindergarten.KindergartenName,
            TeacherName = kindergarten.TeacherName,
            CreatedAt = kindergarten.CreatedAt,
            UpdatedAt = kindergarten.UpdatedAt
        };

        return View(viewmodel);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmation(Guid id)
    {
        var kindergarten = await _kindergartenService.Delete(id);
        
        if (kindergarten == null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var kindergarten = await _kindergartenService.DetailAsync(id);

        if (kindergarten == null)
        {
            return NotFound();
        }

        var viewmodel = new KindergartenDetailsViewmodel
        {
            Id = kindergarten.Id,
            GroupName = kindergarten.GroupName,
            ChildrenCount = kindergarten.ChildrenCount,
            KindergartenName = kindergarten.KindergartenName,
            TeacherName = kindergarten.TeacherName,
            CreatedAt = kindergarten.CreatedAt,
            UpdatedAt = kindergarten.UpdatedAt
        };

        return View(viewmodel);
    }
}