using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    [RegularExpression(@"^[A-Za-z\u0600-\u06FF ]+$", ErrorMessage = "First name can only contain English or Arabic letters and spaces.")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(40)]
    [RegularExpression(@"^[A-Za-z\u0600-\u06FF ]+$", ErrorMessage = "Middle name can only contain English or Arabic letters and spaces.")]
    public string MiddleName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [RegularExpression(@"^[A-Za-z\u0600-\u06FF ]+$", ErrorMessage = "Last name can only contain English or Arabic letters and spaces.")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [CustomValidation(typeof(User), nameof(ValidateBirthDate))]
    public DateOnly BirthDate { get; set; }

    [Required]
    [RegularExpression(@"^\+\d{12}$", ErrorMessage = "Mobile number must be in the format +021006158123.")]
    public string MobileNumber { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public required ICollection<Address> AddressList { get; set; }

    public static ValidationResult? ValidateBirthDate(DateOnly birthDate, ValidationContext context)
    {
        int minAge = 20;
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age)) age--;

        return age >= minAge ? ValidationResult.Success : new ValidationResult("Minimum age is 20 years.");
    }
}

}