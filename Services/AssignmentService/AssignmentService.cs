using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.Assignment;

namespace HomeAssignment.Services.AssignmentService
{
    public class AssignmentService : IAssignmentService
    {
        //Mock assingment
        private static List<Assignment> assignments = new List<Assignment> {
        new Assignment { Title = "Title", Description = "Lorem pusem"}
        };
        
        public async Task<ServiceResponse<List<GetAssignmentDto>>> CreateAssignment(CreateAssignmentDto newAssignment)
        {
            var serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            assignments.Add(newAssignment);
            serviceResponse.Data = assignments;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetAllAssignments()
        {
            return new ServiceResponse<List<GetAssignmentDto>> {Data = assignments};
        }

        public async Task<ServiceResponse<GetAssignmentDto>> GetAssignmentById(int id)
        {
            var serviceResponse = new ServiceResponse<GetAssignmentDto>();
            serviceResponse.Data = assignments.FirstOrDefault(c => c.Id == id);
            return serviceResponse;
        }
    }
}