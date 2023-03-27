using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.Analytics;
using HomeAssignment.Dtos.Assignment;

namespace HomeAssignment.Services.AssignmentService
{
    public interface IAssignmentService
    {
        Task<ServiceResponse<List<GetAssignmentDto>>> GetAllAssignments();
        Task<ServiceResponse<GetAssignmentDto>> GetAssignmentById(int id);
        Task<ServiceResponse<List<GetAssignmentDto>>> CreateAssignment (CreateAssignmentDto newAssignment);
        Task<ServiceResponse<GetAssignmentDto>> UpdateAssignment(UpdateAssignmentDto updateAssignment, int id);
        Task<ServiceResponse<List<GetAssignmentDto>>> DeleteAssignment (int id);
        Task<ServiceResponse<List<GetAssignmentDto>>> GetClosedAssignments();
        Task<ServiceResponse<List<GetAssignmentDto>>> GetOpenAssignments();
        Task<ServiceResponse<List<GetAssignmentDto>>> GetAssignmentsDueThisWeek();
        Task<ServiceResponse<List<GetAssignmentDto>>> GetAssignmentsSortedBy(string sortBy);
        Task<ServiceResponse<List<UserAssignmentDto>>> GetUsersWithMostAssignmentsDone(DateTime startDate, DateTime endDate);
    }
}