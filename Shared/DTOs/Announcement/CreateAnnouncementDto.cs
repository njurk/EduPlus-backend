using System.Collections.Generic;

namespace Shared.DTOs
{
    public class CreateAnnouncementDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public List<int>? RoleIds { get; set; }
    }
}
