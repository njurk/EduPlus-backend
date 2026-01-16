using System;
using System.Collections.Generic;

namespace Shared.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Pesel { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsActive { get; set; }
        public string? Password { get; set; }
        public List<int>? RoleIds { get; set; }
        public List<int>? ChildIds { get; set; }
        public List<int>? ParentIds { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? RoleNames { get; set; } 
        public int? ModifiedByUserId { get; set; }
        public string? ModifiedByName { get; set; }
        public List<string>? Relations { get; set; }
        public List<UserRoleDto>? UserRoles { get; set; }
    }
}
