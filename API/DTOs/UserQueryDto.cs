namespace API.DTOs
{
    public class UserQueryDto
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool SortDesc { get; set; }
        public bool ShowInactive { get; set; }
        public bool OnlyUnassignedParents { get; set; }
        public string? RoleName { get; set; }
    }
}