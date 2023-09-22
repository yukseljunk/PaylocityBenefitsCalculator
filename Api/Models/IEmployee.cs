namespace Api.Models;

public interface IEmployee
{
    int Id { get; set; }
    string? FirstName { get; set; }
    string? LastName { get; set; }
    decimal Salary { get; set; }

    decimal NetSalary { get; set; }
    DateTime DateOfBirth { get; set; }
    ICollection<Dependent> Dependents { get; set; }
}
