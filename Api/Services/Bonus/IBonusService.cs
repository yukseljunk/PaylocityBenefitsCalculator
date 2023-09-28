using Api.Models;
using ErrorOr;

namespace Api.Services;

public interface IBonusService
{
    Task<ErrorOr<Bonus>> CalculateBonus(Models.Employee employee, int weekNumber);

}
