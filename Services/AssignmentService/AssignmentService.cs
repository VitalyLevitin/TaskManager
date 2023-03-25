using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AssignmentService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetAssignmentDto>>> CreateAssignment(CreateAssignmentDto newAssignment)
        {
            var serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            Assignment assignment = _mapper.Map<Assignment>(newAssignment);
            assignment.Id = assignments.Max(c => c.Id) + 1;
            assignments.Add(assignment); //Add to the DB.
            serviceResponse.Data = assignments.Select(c => _mapper.Map<GetAssignmentDto>(c)).ToList(); //Return to the client relevant info.
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetAllAssignments()
        {
            return new ServiceResponse<List<GetAssignmentDto>>
            {
                Data = assignments.Select(c => _mapper.Map<GetAssignmentDto>(c)).ToList()
            };
        }

        public async Task<ServiceResponse<GetAssignmentDto>> GetAssignmentById(int id)
        {
            var serviceResponse = new ServiceResponse<GetAssignmentDto>();
            var assignment = assignments.FirstOrDefault(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetAssignmentDto>(assignment);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetAssignmentDto>> UpdateAssignment(UpdateAssignmentDto updateAssignment)
        {
            ServiceResponse<GetAssignmentDto> response = new ServiceResponse<GetAssignmentDto>();
            try
            {
                Assignment existingAssignment = assignments.FirstOrDefault(c => c.Id == updateAssignment.Id);
                //Wasn't sure if to do this, or iterate with a foreach for each field.
                existingAssignment.Title = updateAssignment.Title;
                existingAssignment.Description = updateAssignment.Description;
                existingAssignment.Status = updateAssignment.Status;
                existingAssignment.Importance = updateAssignment.Importance;
                existingAssignment.UserId = updateAssignment.UserId;
                response.Data = _mapper.Map<GetAssignmentDto>(existingAssignment);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }



    }
}