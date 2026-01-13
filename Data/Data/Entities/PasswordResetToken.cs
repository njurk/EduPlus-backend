using System.ComponentModel.DataAnnotations;

namespace Data.Data.Entities
{
    public class PasswordResetToken
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public required string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
