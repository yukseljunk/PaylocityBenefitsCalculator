using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Dependent;

public record GetDependentDto
{
    public int Id { get; set; }

    [MinLength(2)]
    [MaxLength(20)]
    public string? FirstName { get; set; }

    [MinLength(2)]
    [MaxLength(20)]
    public string? LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship { get; set; }
}
