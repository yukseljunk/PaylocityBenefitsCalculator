using System;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests;

public class DependentIntegrationTests : IntegrationTest
{
    [Fact]
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        await CreateIfEmpty();
        var response = await HttpClient.GetAsync("/api/v1/dependents");
        
        await response.ShouldReturn(HttpStatusCode.OK, Dependents);
    }

    [Fact]
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        await CreateIfEmpty();
        var response = await HttpClient.GetAsync("/api/v1/dependents/1");
        var dependent = new GetDependentDto
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependent);
    }

    [Fact]
    public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
    {
        await CreateIfEmpty();
        var response = await HttpClient.GetAsync($"/api/v1/dependents/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }
}

