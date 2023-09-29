using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Employee;
using Xunit;

namespace ApiTests.IntegrationTests;

public class BonusIntegrationTests : IntegrationTest
{
    [Fact]
    public async Task WhenAskedForBonus_ShouldReturnBonusInfo()
    {
        var employeeId = 1;
        var weekNo = 1;
        await CreateIfEmpty();
        var response = await HttpClient.GetAsync($"/api/v1/bonuses/{employeeId}/week/{weekNo}");
        var bonusDto = new GetBonusDto()
        {
            EmployeeId = employeeId,
            WeekNo = weekNo,
            AmountToPay = 3400.81m,
            Details = new Dictionary<string, decimal> {
                { "Base Salary", 2900.81m},
                { "Base Bonus", 500m}
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, bonusDto);
    }


}

