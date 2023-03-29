using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.Analytics;
using HomeAssignment.Dtos.Assignment;

namespace HomeAssignment.Repository.Assignment
{
    public interface IAssignmentRepository
    {
        Task<List<GetAssignmentDto>> GetAll();
        Task<GetAssignmentDto> GetById(int id);
        Task Create(CreateAssignmentDto newAssignment, User user);
        Task Update(UpdateAssignmentDto updateAssignment, int id);
        Task Delete(Task<GetAssignmentDto> assignment);
        Task<List<GetAssignmentDto>> GetClosed();
        Task<List<GetAssignmentDto>> GetOpen();
        Task<List<GetAssignmentDto>> GetDueThisWeek(DateTime currentDate, DateTime endDate);
        Task<List<GetAssignmentDto>> GetAssignmentsSortedByDueDate();
        Task<List<GetAssignmentDto>> GetAssignmentsSortedByStatus();
        Task<List<GetAssignmentDto>> GetAssignmentsSortedByImportance();
        // Task<List<UserAssignmentDto>> GetUsersWithMostAssignmentsDone(DateTime startDate, DateTime endDate);
    }
}