namespace Shared.DTOs.Mobile
{
    public class MobileSubjectAttendanceDto
    {
        public string SubjectName { get; set; } = "";
        public int TotalLessons { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int Excused { get; set; }
        public double AttendancePercentage { get; set; }
    }
}
