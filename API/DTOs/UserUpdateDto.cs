using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny email")]
        public string Email { get; set; }
        [RegularExpression(@"^[0-9+\- ]*$", ErrorMessage = "Proszę podać poprawny numer telefonu")]
        public string? Phone { get; set; }
        [MaxLength(100)]
        public string? Street { get; set; }
        [MaxLength(50)]
        public string? City { get; set; }
        [MaxLength(10)]
        public string? PostalCode { get; set; }
        public bool IsActive { get; set; }
        [MinLength(8, ErrorMessage = "Hasło musi mieć minimum 8 znaków")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>]).*$", ErrorMessage = "Hasło musi zawierać 1 dużą literę, 1 cyfrę i 1 znak specjalny")]
        public string? Password { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Użytkownik musi mieć rolę")]
        public List<int> RoleIds { get; set; } = new();
    }
}