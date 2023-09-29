using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTests;

public class IntegrationTest : IDisposable
{
    private HttpClient? _httpClient;
    private List<GetEmployeeDto> _employees;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == default)
            {
                _httpClient = new HttpClient
                {
                    //task: update your port if necessary
                    BaseAddress = new Uri("https://localhost:5001")
                };
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            }

            return _httpClient;
        }
    }

    protected List<GetEmployeeDto> Employees
    {
        get
        {
            if (_employees == null)
            {
                _employees = new List<GetEmployeeDto>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "LeBron",
                        LastName = "James",
                        Salary = 75420.99m,
                        DateOfBirth = new DateTime(1984, 12, 30)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Ja",
                        LastName = "Morant",
                        Salary = 92365.22m,
                        DateOfBirth = new DateTime(1999, 8, 10),
                        Dependents = new List<GetDependentDto>
                        {
                            new()
                            {
                                Id = 1,
                                FirstName = "Spouse",
                                LastName = "Morant",
                                Relationship = Relationship.Spouse,
                                DateOfBirth = new DateTime(1998, 3, 3)
                            },
                            new()
                            {
                                Id = 2,
                                FirstName = "Child1",
                                LastName = "Morant",
                                Relationship = Relationship.Child,
                                DateOfBirth = new DateTime(2020, 6, 23)
                            },
                            new()
                            {
                                Id = 3,
                                FirstName = "Child2",
                                LastName = "Morant",
                                Relationship = Relationship.Child,
                                DateOfBirth = new DateTime(2021, 5, 18)
                            }
                        }
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Michael",
                        LastName = "Jordan",
                        Salary = 143211.12m,
                        DateOfBirth = new DateTime(1963, 2, 17),
                        Dependents = new List<GetDependentDto>
                        {
                            new()
                            {
                                Id = 4,
                                FirstName = "DP",
                                LastName = "Jordan",
                                Relationship = Relationship.DomesticPartner,
                                DateOfBirth = new DateTime(1974, 1, 2)
                            }
                        }
                    }
                };

            }
            return _employees;
        }
    }
    protected List<GetDependentDto> Dependents
    {
        get
        {
            return Employees.SelectMany(e => e.Dependents).ToList();
        }
    }

    protected async Task DeleteEmployees()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees");
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<GetEmployeeDto>>>(await response.Content.ReadAsStringAsync());
        if (apiResponse.Data != null)
        {
            foreach (var data in apiResponse.Data)
            {
                var result = await HttpClient.DeleteAsync("/api/v1/employees/" + data.Id);
                //string resultContent = await result.Content.ReadAsStringAsync();
            }
        }

    }
    protected async Task<HttpResponseMessage> CreateEmployee(GetEmployeeDto employee)
    {
        var json = JsonConvert.SerializeObject(employee).ToString();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await HttpClient.PostAsync("/api/v1/employees", content);
    }
    protected async Task<HttpResponseMessage> UpdateEmployee(GetEmployeeDto employee)
    {
        var json = JsonConvert.SerializeObject(employee).ToString();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await HttpClient.PutAsync("/api/v1/employees/" + employee.Id, content);
    }
    protected async Task<HttpResponseMessage> DeleteEmployee(GetEmployeeDto employee)
    {
        return await HttpClient.DeleteAsync("/api/v1/employees/" + employee.Id);
    }

    protected async Task CreateEmployees()
    {

        foreach (var employee in Employees.OrderBy(e=>e.Id))
        {
            await CreateEmployee(employee);
        }

    }
    

    protected async Task CreateIfEmpty()
    {
        await _semaphore.WaitAsync();
        try
        {
            var response = await HttpClient.GetAsync("/api/v1/employees");
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<GetEmployeeDto>>>(await response.Content.ReadAsStringAsync());

            if (apiResponse.Data != null)
            {
                if (apiResponse.Data.Count() == Employees.Count) return;
                await DeleteEmployees();
                await CreateEmployees();
            }
        }
        finally
        {
            _semaphore.Release();
        }
        
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}

