﻿namespace Api.Models;


public class Employee: IEmployee
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Salary { get; set; }
    public decimal NetSalary { get; set; }

    public DateTime DateOfBirth { get; set; }
    public ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();
}
