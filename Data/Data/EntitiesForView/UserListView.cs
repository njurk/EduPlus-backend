namespace Data.Data.EntitiesForView
{
    public class UserListView
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? RoleNames { get; set; }
        public bool IsUnassignedRelation { get; set; }
        public string ModifiedByName { get; set; } = "System";
    }
}
