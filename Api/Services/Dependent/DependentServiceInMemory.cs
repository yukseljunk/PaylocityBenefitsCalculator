using Api.Models;
using Api.ServiceErrors;
using ErrorOr;

namespace Api.Services;

public class DependentServiceInMemory : IDependentService
{
    private static Dictionary<int, Dependent> _data = new();
    public async Task<ErrorOr<Created>> CreateDependent(Dependent dependent)
    {
        var newId = 1;
        if (_data.Any())
        {
            var keys = _data.Keys.ToList();
            newId = keys.Max() + 1;
        }
        _data.Add(newId, dependent);
        dependent.Id = newId;
        return Result.Created;
    }

    public async Task<ErrorOr<Deleted>> DeleteDependent(int id)
    {
        if (!_data.ContainsKey(id)) return DependentErrors.NotFound(id);
        //delete the relationship
        var dependentToDelete = _data[id];
        dependentToDelete.Employee?.Dependents.Remove(dependentToDelete);

        _data.Remove(id);
        return Result.Deleted;
    }

    public async Task<ErrorOr<Dependent>> GetDependent(int id)
    {
        if (_data.ContainsKey(id))
        {
            return _data[id];
        }
        return DependentErrors.NotFound(id) ;
    }

    public async Task<ErrorOr<List<Dependent>>> GetDependents()
    {
        return _data.Values.ToList();
    }

    public async Task<ErrorOr<Updated>> Update(Dependent dependent)
    {
        if (_data.ContainsKey(dependent.Id))
        {
            //update the employee dependent object
            var employee = _data[dependent.Id].Employee;
            var employeeDependent = employee?.Dependents.FirstOrDefault(d => d.Id == dependent.Id);
            if (employeeDependent != null)
            {
                employee?.Dependents.Remove(employeeDependent);
                employee?.Dependents.Add(dependent);
            }

            _data[dependent.Id] = dependent;
            return Result.Updated;

        }
        return DependentErrors.NotFound(dependent.Id);
    }
}