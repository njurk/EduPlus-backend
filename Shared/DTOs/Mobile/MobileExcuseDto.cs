namespace Shared.DTOs.Mobile
{
    public class MobileExcuseDto
    {
        public int Id { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusColorHex { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<MobileExcuseAttendanceDto> Attendances { get; set; } = new();
    }
}
