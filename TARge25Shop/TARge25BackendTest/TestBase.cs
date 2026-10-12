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
using TARge25BackendTest.mock;
 using TARge25BackendTest.macros;

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

     private void SetUpServices(ServiceCollection services)
    {
        services.AddScoped<IFileServices, FileServices>();
        services.AddScoped<IKindergartenServiceInterface, KindergartenServices>();
        services.AddDbContext<TARge25ShopContext>(options =>
            options.UseInMemoryDatabase("InMemoryDbForTesting")
        );
        
        services.AddScoped<IHostEnvironment, MockIHostEnvironment>();
        RegisterMacros(services);
    }
    
    protected T Svc<T>()
    {
        return ServiceProvider.GetRequiredService<T>();
    }
    
    public void Dispose()
    {
        var dbContext = ServiceProvider.GetRequiredService<TARge25ShopContext>();
        dbContext.Database.EnsureDeleted();
    }
    
    private void RegisterMacros(ServiceCollection services)
    {
        var macroBaseType = typeof(IMacros);

        var macros = macroBaseType.Assembly.GetTypes()
            .Where(t => macroBaseType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var macro in macros)
        {
            services.AddSingleton(macro);
        }
    }
}