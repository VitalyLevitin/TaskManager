using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using HomeAssignment.Data;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.Assignment;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Services.AssignmentService
{
    public class AssignmentService : IAssignmentService
    {
        //Mock assingment
        private static List<Assignment> assignments = new List<Assignment> {
        new Assignment { Title = "Title", Description = "Lorem pusem"}
        };
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public AssignmentService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
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

        public async Task<ServiceResponse<List<GetAssignmentDto>>> DeleteAssignment(int id)
        {
            ServiceResponse<List<GetAssignmentDto>> response = new ServiceResponse<List<GetAssignmentDto>>();

            try
            {
                Assignment assignment = assignments.First(c => c.Id == id);
                assignments.Remove(assignment);
                response.Data = assignments.Select(c => _mapper.Map<GetAssignmentDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetAllAssignments()
        {
            var response = new ServiceResponse<List<GetAssignmentDto>>();
            var dbAssignments = await _context.Assignments.ToListAsync();
            response.Data = dbAssignments.Select(c => _mapper.Map<GetAssignmentDto>(c)).ToList();
            return response;
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
            Assignment? existingAssignment = assignments.FirstOrDefault(c => c.Id == updateAssignment.Id);
            if (existingAssignment == null)
            {
                response.Success = false;
                response.Message = $"No user with {updateAssignment.Id} found";
                return response;
            }
            _mapper.Map(updateAssignment, existingAssignment);
            response.Data = _mapper.Map<GetAssignmentDto>(existingAssignment);
            return response;
        }



    }
}