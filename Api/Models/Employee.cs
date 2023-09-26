using Api.ServiceErrors;
using Api.Services;
using ErrorOr;
using System.Xml.Linq;

namespace Api.Models;


public class Employee : IEmployee
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Salary { get; set; }


    public DateTime DateOfBirth { get; set; }
    public ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();

    public decimal MonthlySalary { get; set; }

    private Employee(int id, string? firstName, string? lastName, decimal salary, DateTime dateOfBirth, ICollection<Dependent> dependents)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Salary = salary;
        DateOfBirth = dateOfBirth;
        Dependents = dependents;
    }

    //TODO: move this to a factory class to keep this class SRP
    public static ErrorOr<Employee> Create(string? firstName, string? lastName, decimal salary, DateTime dateOfBirth, ICollection<Dependent> dependents, int? id = null)
    {
        //complex validations here
        List<Error> errors = new();

        var invalidDependents = dependents.Where(d => !Enum.IsDefined(typeof(Relationship), d.Relationship));
        if (invalidDependents.Any())
        {
            errors.Add(DependentErrors.InvalidRelationshipDependent(invalidDependents.Select(d => d.Id).ToArray()));
        }


        var spouseDependentCount = dependents.Count(d => d.Relationship == Relationship.Spouse || d.Relationship == Relationship.DomesticPartner);
        if (spouseDependentCount > 1)
        {
            errors.Add(EmployeeErrors.NotMoreThanOneSpouse);
        }

        var noRelationshipDependents = dependents.Where(d => d.Relationship == Relationship.None);
        if (noRelationshipDependents.Any())
        {
            errors.Add(DependentErrors.NoRelationshipDependent(noRelationshipDependents.Select(d => d.Id).ToArray()));
        }

        if (id != null && id.Value > 0)
        {
            //update validations

            var dependentIdDuplicates = dependents.GroupBy(d => d.Id).Where(dg => dg.Count() > 1);
            if (dependentIdDuplicates.Any())
            {
                errors.Add(DependentErrors.DuplicateId(dependentIdDuplicates.Select(d => d.Key).ToArray()));
            }
        }

        if (errors.Any())
        {
            return errors;
        }

        return new Employee(
            id ?? 0,
            firstName,
            lastName,
            salary,
            dateOfBirth,
            dependents
            );
    }
}
