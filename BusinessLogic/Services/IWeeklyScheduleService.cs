using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IWeeklyScheduleService
    {
        Task<List<LessonDto>> GetScheduleForClassAsync(int classId, DateTime dateFrom, DateTime dateTo);
        // Task<List<LessonDto>> GetScheduleForTeacherAsync(int teacherId, DateTime dateFrom, DateTime dateTo); // opcjonalnie
    }
}
