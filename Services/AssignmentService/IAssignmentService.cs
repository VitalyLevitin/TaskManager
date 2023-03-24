using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.Assignment;

namespace HomeAssignment.Services.AssignmentService
{
    public interface IAssignmentService
    {
        Task<ServiceResponse<List<GetAssignmentDto>>> GetAllAssignments();
        Task<ServiceResponse<GetAssignmentDto>> GetAssignmentById(int id);
        Task<ServiceResponse<List<GetAssignmentDto>>> CreateAssignment (CreateAssignmentDto newAssignment);
    }
}