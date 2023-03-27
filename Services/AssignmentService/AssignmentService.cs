using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HomeAssignment.Data;
using HomeAssignment.Domain;
using HomeAssignment.Domain.Enums;
using HomeAssignment.Dtos.Analytics;
using HomeAssignment.Dtos.Assignment;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Services.AssignmentService
{
    public class AssignmentService : IAssignmentService
    {
        //Mock assingment
        // private static List<Assignment> assignments = new List<Assignment> {
        // new Assignment { Title = "Title", Description = "Lorem pusem"}
        // };
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
            User user = await _context.Users.FirstAsync(c => c.Id == newAssignment.UserId);
            assignment.AssignedToUser = user.Username;
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Assignments
                .Select(c => _mapper.Map<GetAssignmentDto>(c)).ToListAsync(); //Return to the client relevant info.
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> DeleteAssignment(int id)
        {
            ServiceResponse<List<GetAssignmentDto>> response = new ServiceResponse<List<GetAssignmentDto>>();
            try
            {
                Assignment assignment = await _context.Assignments.FirstAsync(c => c.Id == id);
                _context.Remove(assignment);
                await _context.SaveChangesAsync();
                response.Data = await _context.Assignments
                    .Select(c => _mapper.Map<GetAssignmentDto>(c)).ToListAsync();
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
            var dbAssignment = await _context.Assignments.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetAssignmentDto>(dbAssignment);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetClosedAssignments()
        {
            var serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            var dbAssignments = await _context.Assignments.ToListAsync();
            var pendingAssignments = dbAssignments
                .Where(a => a.Status == Status.Done)
                .Select(a => _mapper.Map<GetAssignmentDto>(a))
                .ToList();
            serviceResponse.Data = pendingAssignments;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetOpenAssignments()
        {
            var serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            var dbAssignments = await _context.Assignments.ToListAsync();
            var pendingAssignments = dbAssignments
                .Where(a => a.Status != Status.Done)
                .Select(a => _mapper.Map<GetAssignmentDto>(a))
                .ToList();
            serviceResponse.Data = pendingAssignments;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetAssignmentsDueThisWeek()
        {
            var serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            var dbAssignments = await _context.Assignments.ToListAsync();

            // Calculate start and end dates of current week
            var currentDate = DateTime.Now;
            var endDate = currentDate.AddDays(7);

            // Filter assignments by due date
            var assignmentsDueThisWeek = dbAssignments
                .Where(a => a.DueDate >= currentDate && a.DueDate <= endDate)
                .Select(a => _mapper.Map<GetAssignmentDto>(a))
                .ToList();

            serviceResponse.Data = assignmentsDueThisWeek;
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetAssignmentsSortedBy(string sortBy)
        {
            var serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();

            // Make sure we don't get invalid sorting param.
            string CapitalizeString = sortBy.Substring(0, 1).ToUpper() + sortBy.Substring(1);
            if (!Enum.TryParse(CapitalizeString, out SortBy sortByEnum))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid sort by parameter";
                return serviceResponse;
            }

            var dbAssignments = await _context.Assignments.ToListAsync();

            // Sort assignments by the specified property
            switch (sortBy.ToLower())
            {
                case "date":
                    dbAssignments = dbAssignments.OrderBy(a => a.DueDate).ToList();
                    break;
                case "status":
                    dbAssignments = dbAssignments.OrderByDescending(a => a.Status).ToList();
                    break;
                case "importance":
                    dbAssignments = dbAssignments.OrderByDescending(a => a.Importance).ToList();
                    break;
                default:
                    break;
            }

            // Map assignments to DTOs
            var assignments = dbAssignments.Select(a => _mapper.Map<GetAssignmentDto>(a)).ToList();

            serviceResponse.Data = assignments;
            return serviceResponse;
        }


        public async Task<ServiceResponse<GetAssignmentDto>> UpdateAssignment(UpdateAssignmentDto updateAssignment, int id)
        {
            ServiceResponse<GetAssignmentDto> response = new ServiceResponse<GetAssignmentDto>();
            Assignment? existingAssignment = await _context.Assignments
                .FirstOrDefaultAsync(c => c.Id == id);
            if (existingAssignment is null)
            {
                response.Success = false;
                response.Message = $"No user with id {id} found";
                return response;
            }

            _mapper.Map(updateAssignment, existingAssignment);
            response.Data = _mapper.Map<GetAssignmentDto>(existingAssignment);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<List<UserAssignmentDto>>> GetUsersWithMostAssignmentsDone(DateTime startDate, DateTime endDate)
        {
            var serviceResponse = new ServiceResponse<List<UserAssignmentDto>>();
            var assignments = await _context.Assignments
                .Where(a => a.Status == Status.Done && a.DateCreated >= startDate && a.DateCreated <= endDate)
                .ToListAsync();

            var userAssignments = assignments.GroupBy(a => a.UserId)
                .Select(g => new UserAssignmentDto
                {
                    UserId = g.Key,
                    Username = g.FirstOrDefault()?.AssignedToUser!,
                    NumberOfDoneAssignments = g.Count()
                })
                .OrderByDescending(u => u.NumberOfDoneAssignments)
                .ToList();

            serviceResponse.Data = userAssignments;
            return serviceResponse;
        }


    }
}