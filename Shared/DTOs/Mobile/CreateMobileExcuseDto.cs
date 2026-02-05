namespace Shared.DTOs.Mobile
{
    public class CreateMobileExcuseDto
    {
        public List<int> AttendanceIds { get; set; } = new();
        public string Reason { get; set; } = "";
    }
}
