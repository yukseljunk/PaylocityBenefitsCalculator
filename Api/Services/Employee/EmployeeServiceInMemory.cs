﻿using Api.Models;
using Api.ServiceErrors;
using ErrorOr;

namespace Api.Services.Employee;

public class EmployeeServiceInMemory : IEmployeeService
{
    public static readonly object _locker = new object();

    private static Dictionary<int, Models.Employee> _data = new();
    public IDependentService _dependentService;

    public EmployeeServiceInMemory(IDependentService dependentService)
    {
        _dependentService = dependentService;

    }

    public async Task<ErrorOr<Created>> CreateEmployee(Models.Employee employee)
    {
        var newId = 1;
        lock (_locker)
        {
            if (_data.Any())
            {
                var keys = _data.Keys.ToList();
                newId = keys.Max() + 1;
            }
            _data.Add(newId, employee);
        }
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
        List<Dependent> dependents;
        lock (_locker)
        {
            if (!_data.ContainsKey(id)) return EmployeeErrors.NotFound(id);
            dependents = _data[id].Dependents.ToList();
        }
        foreach (var dependent in dependents)
        {
            var result = await _dependentService.DeleteDependent(dependent.Id);
            if (result == DependentErrors.NotFound(id)) return DependentErrors.NotFound(id);
        }
        lock (_locker) { _data.Remove(id); }
        return Result.Deleted;
    }

    public async Task<ErrorOr<Models.Employee>> GetEmployee(int id)
    {
        lock (_locker)
        {
            if (_data.ContainsKey(id))
            {
                return _data[id];
            }
        }
        return EmployeeErrors.NotFound(id);
    }

    public async Task<ErrorOr<List<Models.Employee>>> GetEmployees()
    {
        lock (_locker)
        {
            return _data.Values.OrderBy(e => e.Id).ToList();
        }
    }

    //Can be replaced with upsert, then no need to check for existence,
    //but a new dto needed to pass back to determine if it is update or create operation
    //TODO: THis is buggy: an employee with a dependent id 1, another employee with a dep id 2; update first employee with dep1 should not be possible
    //so logic needs to be using dependent service
    public async Task<ErrorOr<Updated>> Update(Models.Employee employee)
    {
        ICollection<Dependent> dependents;
        lock (_locker)
        {
            if (!_data.ContainsKey(employee.Id)) return EmployeeErrors.NotFound(employee.Id);
            dependents = _data[employee.Id].Dependents;
        }

        //dependent operations
        var newDependents = employee.Dependents;

        var removedDependentIds = dependents.Select(nd => nd.Id).Except(newDependents.Select(n => n.Id)).ToList();
        var existentDependentIds = dependents.Select(nd => nd.Id).Intersect(newDependents.Select(n => n.Id)).ToList();

        //deleted dependents
        foreach (var dependentId in removedDependentIds)
        {
            var deleteResult = await _dependentService.DeleteDependent(dependentId);
            if (deleteResult == DependentErrors.NotFound(dependentId)) return DependentErrors.NotFound(dependentId);
        }
        //updated dependents
        foreach (var dependentId in existentDependentIds)
        {
            var updateResult = await _dependentService.Update(newDependents.First(d => d.Id == dependentId));
            if (updateResult == DependentErrors.NotFound(dependentId)) return DependentErrors.NotFound(dependentId);
        }

        //rest are new dependents
        foreach (var dependent in newDependents)
        {
            if (removedDependentIds.Contains(dependent.Id) || existentDependentIds.Contains(dependent.Id)) continue;
            await _dependentService.CreateDependent(dependent);
        }

        //set newdependent's employee fields 
        foreach (var dependent in newDependents)
        {
            dependent.EmployeeId = employee.Id;
            dependent.Employee = employee;
        }
        lock (_locker) _data[employee.Id] = employee;
        return Result.Updated;
    }


}