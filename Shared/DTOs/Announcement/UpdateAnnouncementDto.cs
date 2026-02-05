using System.Collections.Generic;

namespace Shared.DTOs
{
    public class UpdateAnnouncementDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<int>? RoleIds { get; set; }
    }
}
