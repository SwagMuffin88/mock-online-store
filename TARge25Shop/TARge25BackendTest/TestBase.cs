﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core;
using TARge25Shop.Data;
using TARge25Shop.SpaceshipTest.Mock;

namespace TARge25BackendTest;

public abstract class TestBase : IDisposable
{
    protected IServiceProvider ServiceProvider { get; private set; }

    protected TestBase()
    {
        var services = new ServiceCollection();
        SetUpServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    protected void SetUpServices(IServiceCollection services)
    {
        services.AddScoped<IFileServices, FileServices>();
        services.AddScoped<IKindergartenServiceInterface, KindergartenServices>();
        services.AddDbContext<TARge25ShopContext>(options =>
            options.UseInMemoryDatabase("InMemoryDbForTesting")
        );
        
        services.AddScoped<IHostEnvironment, MockIHostEnvironment>();
    }
    
    public void Dispose()
    {
        var dbContext = ServiceProvider.GetRequiredService<TARge25ShopContext>();
        dbContext.Database.EnsureDeleted();
    }
}