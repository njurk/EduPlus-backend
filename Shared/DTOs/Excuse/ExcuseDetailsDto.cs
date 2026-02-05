namespace Shared.DTOs
{
    public class ExcuseDetailsDto : ExcuseDto
    {
        public List<ExcuseAttendanceItemDto> Attendances { get; set; } = new();
    }
}
