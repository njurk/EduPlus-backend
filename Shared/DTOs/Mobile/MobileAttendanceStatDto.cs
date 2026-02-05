namespace Shared.DTOs.Mobile
{
    public class MobileAttendanceStatDto
    {
        public string ShortCode { get; set; } = "";
        public string Name { get; set; } = "";
        public string ColorHex { get; set; } = "";
        public int Count { get; set; }
        public bool IsNegative { get; set; }
    }
}
