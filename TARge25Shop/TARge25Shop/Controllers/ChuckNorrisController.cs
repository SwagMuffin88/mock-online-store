using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core;
using TARge25Shop.Core.Dto;
using TARge25Shop.Models.ChuckNorris;

namespace TARge25Shop.Controllers;

public class ChuckNorrisController : Controller
{ 
    private readonly IChuckNorrisServices _chuckNorrisService;
    private readonly ILogger<ChuckNorrisController> _logger;
    
    public ChuckNorrisController(IChuckNorrisServices chuckNorrisService, ILogger<ChuckNorrisController> logger)
    {
        _chuckNorrisService = chuckNorrisService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _chuckNorrisService.ChuckNorrisResultHttpClient();
        
        if (response == null)
        {
            _logger.LogWarning("Could not get data from service");
            return View(new ChuckNorrisViewModel());
        }
        
        var viewmodel = new ChuckNorrisViewModel
        {
            Id = response.Id,
            Categories = response.Categories,
            CreatedAt = response.CreatedAt,
            Url = response.Url,
            UpdatedAt = response.UpdatedAt,
            IconUrl = response.IconUrl,
            Value = response.Value
        };
        return View(viewmodel);
    }

    [HttpGet]
    public IActionResult GetNewJoke()
    {
        return RedirectToAction("Index");
    }
}