using ErrorOr;

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
        Salary= salary;
        DateOfBirth = dateOfBirth;  
        Dependents = dependents;
    }

    //TODO: move this to a factory class to keep this class SRP
    public static ErrorOr<Employee> Create(string? firstName, string? lastName, decimal salary, DateTime dateOfBirth, ICollection<Dependent> dependents, int? id = null)
    {
        //complex validations here
        List<Error> errors = new();
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
