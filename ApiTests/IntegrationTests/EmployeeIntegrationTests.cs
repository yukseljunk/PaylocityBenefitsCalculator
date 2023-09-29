using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Dtos.Employee;
using Newtonsoft.Json;
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

    [Fact]
    public async Task WhenAskedForCreatingEmployee_ShouldReturnCreated()
    {
        await CreateIfEmpty();
        var employeeDto = new GetEmployeeDto() { FirstName = "John", LastName = "Doe", Salary = 78000m };
        var response = await CreateEmployee(employeeDto);
        await response.ShouldReturn(HttpStatusCode.Created);
        await DeleteEmployees();
    }

    [Fact]
    public async Task WhenAskedForCreatingInvalidEmployee_ShouldReturnBadRequest()
    {
        await CreateIfEmpty();
        var employeeDto = new GetEmployeeDto() { FirstName = "A", LastName = "B", Salary = 78000m };
        var response = await CreateEmployee(employeeDto);
        await response.ShouldReturn(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task WhenAskedForUpdatingEmployee_ShouldReturnNoContent()
    {
        await CreateIfEmpty();
        var employeeDto = Employees.First();
        employeeDto.FirstName = "NewName";
        var response = await UpdateEmployee(employeeDto);
        await response.ShouldReturn(HttpStatusCode.NoContent);
        await DeleteEmployees();
    }

    [Fact]
    public async Task WhenAskedForDeletingEmployee_ShouldReturnNoContent()
    {
        await CreateIfEmpty();
        var employeeDto = Employees.First();
        var response = await DeleteEmployee(employeeDto);
        await response.ShouldReturn(HttpStatusCode.NoContent);
        await DeleteEmployees();
    }
}

