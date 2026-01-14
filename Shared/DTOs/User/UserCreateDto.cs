namespace Shared.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class UserCreateDto
    {
        [Required(ErrorMessage = "Imiê jest wymagane")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny email")]
        public string Email { get; set; }

        [RegularExpression(@"^[0-9+\- ]*$", ErrorMessage = "Proszê podaæ poprawny numer telefonu")]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Street { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(10)]
        public string? PostalCode { get; set; }

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Has³o jest wymagane")]
        [MinLength(8, ErrorMessage = "Has³o musi mieæ minimum 8 znaków")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>]).*$", ErrorMessage = "Has³o musi zawieraæ 1 du¿¹ literê, 1 cyfrê i 1 znak specjalny")]
        public string Password { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "U¿ytkownik musi mieæ rolê")]
        public List<int> RoleIds { get; set; } = new();

        public List<int> ChildIds { get; set; } = new List<int>();
    }
}
