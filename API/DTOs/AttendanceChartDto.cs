namespace API.DTOs
{
    public class AttendanceChartDto
    {
        public string Date { get; set; } = string.Empty;

        public string DayName { get; set; } = string.Empty;

        public int AttendancePercentage { get; set; }
    }
}
