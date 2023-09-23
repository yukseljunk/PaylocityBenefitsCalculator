using Api.ServiceErrors;
using ErrorOr;

namespace Api.Services.Employee;

public class EmployeeServiceInMemory : IEmployeeService
{
    private static Dictionary<int, Models.Employee> _data = new();
    private IDependentService _dependentService;

    public EmployeeServiceInMemory(IDependentService dependentService)
    {
        _dependentService = dependentService;
    }

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

        foreach (var dependent in employee.Dependents)
        {
            await _dependentService.CreateDependent(dependent);
            dependent.EmployeeId = newId;
            dependent.Employee = employee;
        }

        return Result.Created;
    }

    public async Task<ErrorOr<Deleted>> DeleteEmployee(int id)
    {
        if (!_data.ContainsKey(id)) return EmployeeErrors.NotFound;

        foreach (var dependent in _data[id].Dependents)
        {
            var result = await _dependentService.DeleteDependent(dependent.Id);
            if (result == DependentErrors.NotFound) return DependentErrors.NotFound;
        }
        _data.Remove(id);
        return Result.Deleted;
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
        if (!_data.ContainsKey(employee.Id)) return EmployeeErrors.NotFound;


        //dependent operations
        var dependents = _data[employee.Id].Dependents;
        var newDependents = employee.Dependents;

        var removedDependentIds = newDependents.Select(nd => nd.Id).Except(dependents.Select(n => n.Id));
        var existentDependentIds = dependents.Select(nd => nd.Id).Intersect(newDependents.Select(n => n.Id));

        foreach (var dependentId in removedDependentIds)
        {
            var deleteResult = await _dependentService.DeleteDependent(dependentId);
            if (deleteResult == DependentErrors.NotFound) return DependentErrors.NotFound;
        }
        foreach (var dependentId in existentDependentIds)
        {
            var updateResult = await _dependentService.Update(newDependents.First(d => d.Id == dependentId));
            if (updateResult == DependentErrors.NotFound) return DependentErrors.NotFound;
        }

        foreach (var dependent in dependents)
        {
            if (removedDependentIds.Contains(dependent.Id) || existentDependentIds.Contains(dependent.Id)) continue;
            await _dependentService.CreateDependent(dependent);
        }

        _data[employee.Id] = employee;
        return Result.Updated;
    }


}