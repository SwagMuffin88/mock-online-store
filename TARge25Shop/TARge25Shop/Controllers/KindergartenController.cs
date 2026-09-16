using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core;
using TARge25Shop.Data;

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
}