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
 using TARge25Shop.Core.Domain;
 using TARge25Shop.Core.Dto;

 namespace TARge25BackendTest;

public abstract class TestBase : IDisposable
{
    protected IServiceProvider ServiceProvider { get; private set; }
    protected TARge25ShopContext DbContext { get; private set; }
    
    protected TestBase()
    {
        var services = new ServiceCollection();
        
        SetUpServices(services);
        ServiceProvider = services.BuildServiceProvider();
        
        DbContext = ServiceProvider.GetRequiredService<TARge25ShopContext>();
        DbContext.Database.EnsureCreated();
    }

     private void SetUpServices(ServiceCollection services)
    {
        services.AddScoped<IFileServices, FileServices>();
        services.AddScoped<IKindergartenServiceInterface, KindergartenServices>();
        services.AddDbContext<TARge25ShopContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString())
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
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
        
        if (ServiceProvider is IDisposable disposableProvider)
        {
            disposableProvider.Dispose();
        }
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
    
    protected async Task<Kindergarten> SeedKindergartenAsync(
        Guid? id = null, 
        string kindergartenName = "Seedkindergarten", 
        string groupName = "Hello test")
    {
        var kindergarten = new Kindergarten
        {
            Id = id ?? Guid.NewGuid(),
            KindergartenName = kindergartenName,
            GroupName = groupName,
            ChildrenCount = 15,
            TeacherName = "Mari Maasikas",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        
        DbContext.Kindergartens.Add(kindergarten);
        await DbContext.SaveChangesAsync();
        
        DbContext.ChangeTracker.Clear();

        return kindergarten;
    }

    protected KindergartenDto MockKindergartenDtoData(Guid? id = null)
    {
        return new KindergartenDto
        {
            Id = id ?? Guid.NewGuid(),
            KindergartenName = "Kindergarten dto test",
            GroupName = "Dto group",
            ChildrenCount = 15,
            TeacherName = "Mari Maasikas",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    protected KindergartenDto MockKindergartenDtoDataFromExistingData(Kindergarten existing)
    {
        return new KindergartenDto
        {
            Id = existing.Id,
            KindergartenName = existing.KindergartenName,
            GroupName = existing.GroupName,
            ChildrenCount = existing.ChildrenCount,
            TeacherName = existing.TeacherName,
            CreatedAt = existing.CreatedAt,
            UpdatedAt = DateTime.Now
        };
    }
}