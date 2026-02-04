namespace Shared.DTOs.Mobile
{
    public class MobileAttendanceDto
    {
        public List<MobileSubjectAttendanceDto> Subjects { get; set; } = new();
        public List<MobileAttendanceRecordDto> RecentRecords { get; set; } = new();
    }

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

    public class MobileAttendanceRecordDto
    {
        public string SubjectName { get; set; } = "";
        public string Date { get; set; } = "";
        public string Type { get; set; } = "";
        public string TypeColorHex { get; set; } = "";
    }
}
