using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }

        public string? Password { get; set; }

        public bool? IsActive { get; set; }

        public List<int>? RoleIds { get; set; }

        public List<int>? ChildIds { get; set; }

        public List<int>? ParentIds { get; set; }
    }
}
