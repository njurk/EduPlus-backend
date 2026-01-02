namespace API.DTOs
{
    public class DashboardStatusDto
    {
        public string SchoolYear { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public string AvgAttendanceToday { get; set; } = string.Empty;
        public string AvgGradeThisSemester { get; set; } = string.Empty;
    }
}
