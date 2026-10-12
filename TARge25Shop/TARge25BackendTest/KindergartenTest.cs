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
        
        //await Assert.ThrowsAsync<ArgumentException>(async () => await service.Create(dto));
        Assert.Null(result);
    }
    
    [Fact]
    public async Task ShouldNot_GetKindergartenById_WhenIdNonexistent()
    {
        Guid wrongGuid = Guid.NewGuid();

        var result = await Svc<IKindergartenServiceInterface>().DetailAsync(wrongGuid);

        Assert.Null(result);
    }
    
    
}