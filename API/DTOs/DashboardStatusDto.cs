namespace API.DTOs
{
    public class DashboardStatusDto
    {
        public string SchoolYear { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public string AvgAttendance { get; set; } = string.Empty;
        public string AvgGrade { get; set; } = string.Empty;
    }
}
