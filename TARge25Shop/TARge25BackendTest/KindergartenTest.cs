﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core;
using TARge25Shop.Core.Dto;
using TARge25Shop.Data;
using Xunit;

namespace TARge25BackendTest;

public class KindergartenTest : TestBase
{
    [Fact]
    public async Task Should_GetKindergartenDetails_WhenIdExists()
    {
        Guid expectedId = Guid.NewGuid();
        string expectedName = "Excpected kindergarten";
        string expectedGroupName = "Excpected group";
        
        await SeedKindergartenAsync(id: expectedId, kindergartenName: expectedName, groupName: expectedGroupName);

        var result = await Svc<IKindergartenServiceInterface>().DetailAsync(expectedId);
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        Assert.Equal(expectedName, result.KindergartenName);
        Assert.Equal(expectedGroupName, result.GroupName);
    }

    [Fact]
    public async Task ShouldNot_GetKindergartenDetails_WhenIdDoesNotExist()
    {
        Guid wrongId = new Guid();
        
        var result = await Svc<IKindergartenServiceInterface>().DetailAsync(wrongId);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task ShouldNot_GetKindergartenDetails_WhenIdIsEmptyGuid()
    {
        Guid emptyGuid = Guid.Empty;

        var result = await Svc<IKindergartenServiceInterface>().DetailAsync(emptyGuid);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task Should_AddNonEmptyKindergarten_WhenResultIsReturned()
    {
        KindergartenDto dto = new KindergartenDto
        {
            ChildrenCount = 10,
            GroupName = "Testgroup",
            TeacherName = "Jane Doe",
            KindergartenName = "Kindergarten 1",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };

        var result = await Svc<IKindergartenServiceInterface>().Create(dto);
        
        Assert.NotNull(result);
        Assert.Equal("Testgroup", result.GroupName);
    }

    [Fact]
    public async Task ShouldNot_AddEmptyKindergarten_WhenResultIsReturned()
    {
        var service = Svc<IKindergartenServiceInterface>();
        KindergartenDto dto = new KindergartenDto();
        
        var result = await service.Create(dto);
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task ShouldNot_GetKindergartenById_WhenIdNonexistent()
    {
        Guid wrongGuid = Guid.NewGuid();

        var result = await Svc<IKindergartenServiceInterface>().DetailAsync(wrongGuid);

        Assert.Null(result);
    }

    [Fact]
    public async Task Should_GetKindergartenById_WhenIdExists()
    {
        Guid expectedId = Guid.NewGuid();
        string expectedName = "Excpected";
    
        await SeedKindergartenAsync(id: expectedId, kindergartenName: expectedName);

        var result = await Svc<IKindergartenServiceInterface>().DetailAsync(expectedId);

        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        Assert.Equal(expectedName, result.KindergartenName);
    }

    [Fact]
    public async Task Should_DeleteKindergartenById_WhenIdExists()
    {
        var seededKindergarten = await SeedKindergartenAsync();
        
        var result = await Svc<IKindergartenServiceInterface>().Delete(seededKindergarten.Id);

        Assert.NotNull(result);
        Assert.Equal(seededKindergarten.Id, result.Id);
    }

    [Fact]
    public async Task ShouldNot_DeleteKindergarten_WhenIdDoesNotExist()
    {
        Guid wrongGuid = Guid.NewGuid();

        var result = await Svc<IKindergartenServiceInterface>().Delete(wrongGuid);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task Should_UpdateKindergarten_WhenIdExists()
    {
        var existing = await SeedKindergartenAsync();
        var dto = MockKindergartenDtoDataFromExistingData(existing);

        var result = Svc<IKindergartenServiceInterface>().Update(dto);
        
        Assert.NotNull(result);
        Assert.Equal(existing.KindergartenName,  dto.KindergartenName);
    }

    [Fact]
    public async Task ShouldNot_UpdateKindergarten_WhenIdDoesNotExist()
    {
        Guid wrongId = new Guid();
        var existing = await SeedKindergartenAsync();

        var updateDto = new KindergartenDto
        {
            Id = wrongId,
            KindergartenName = "Nonexisting",
            GroupName = "Hello test"
        };

        var result = await Svc<IKindergartenServiceInterface>().Update(updateDto);
    }

    [Fact]
    public async Task ShouldNot_UpdateKindergarten_WhenNameIsEmpty()
    {
        var existing = await SeedKindergartenAsync();

        var invalidDto = new KindergartenDto
        {
            Id = existing.Id,
            KindergartenName = "",
            GroupName = "DtoGroup"
        };

        var result = await Svc<IKindergartenServiceInterface>().Update(invalidDto);
        
        Assert.Null(result);
    }
    
    
}