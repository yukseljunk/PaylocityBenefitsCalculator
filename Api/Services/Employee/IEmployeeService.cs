using ErrorOr;

namespace Api.Services;

public interface IEmployeeService
{
    Task<ErrorOr<Created>> CreateEmployee(Models.Employee employee);

    Task<ErrorOr<Deleted>> DeleteEmployee(int id);

    Task<ErrorOr<Models.Employee>> GetEmployee(int id);

    Task<ErrorOr<Updated>> Update(Models.Employee employee);

    Task<ErrorOr<List<Models.Employee>>> GetEmployees();

}
