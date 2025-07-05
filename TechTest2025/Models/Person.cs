using System;
using System.ComponentModel.DataAnnotations;

namespace TechTest2025.Models
{

    public class Person
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;


        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Person), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        public static ValidationResult ValidateDateOfBirth(DateTime dob, ValidationContext context)
        {
            return dob < DateTime.Today ? ValidationResult.Success : new ValidationResult("Date of birth must be in the past.");
        }
    }
}
