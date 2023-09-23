using Api.ServiceErrors;
using ErrorOr;

namespace Api.Services.Employee;

public class EmployeeServiceInMemory : IEmployeeService
{
    private static Dictionary<int, Models.Employee> _data = new();

    public async Task<ErrorOr<Created>> CreateEmployee(Models.Employee employee)
    {
        var newId = 1;
        if (_data.Any())
        {
            var keys = _data.Keys.ToList();
            newId = keys.Max() + 1;
        }
        _data.Add(newId, employee);
        employee.Id = newId;
        return Result.Created;
    }

    public async Task<ErrorOr<Deleted>> DeleteEmployee(int id)
    {
        if (_data.ContainsKey(id))
        {
            _data.Remove(id);
            return Result.Deleted;
        }
        return EmployeeErrors.NotFound;
    }

    public async Task<ErrorOr<Models.Employee>> GetEmployee(int id)
    {
        if (_data.ContainsKey(id))
        {
            return _data[id];
        }
        return EmployeeErrors.NotFound;
    }

    public async Task<ErrorOr<List<Models.Employee>>> GetEmployees()
    {
        return _data.Values.ToList();
    }

    //Can be replaced with upsert, then no need to check for existence,
    //but a new dto needed to pass back to determine if it is update or create operation
    public async Task<ErrorOr<Updated>> Update(Models.Employee employee)
    {
        if (_data.ContainsKey(employee.Id))
        {
            _data[employee.Id] = employee;
            return Result.Updated;

        }
        return EmployeeErrors.NotFound;
    }

   
}