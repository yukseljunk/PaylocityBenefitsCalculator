using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Dtos.Employee;
using Xunit;

namespace ApiTests.IntegrationTests;


public class EmployeeIntegrationTests : IntegrationTest
{

    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        await CreateIfEmpty();

        var response = await HttpClient.GetAsync("/api/v1/employees");
        await response.ShouldReturn(HttpStatusCode.OK, Employees);
    }


    [Fact]
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        await CreateIfEmpty();
        var response = await HttpClient.GetAsync("/api/v1/employees/1");
        string resultContent = await response.Content.ReadAsStringAsync();
        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        };
        await response.ShouldReturn(HttpStatusCode.OK, employee);
    }

    [Fact]
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        await CreateIfEmpty();
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

}

