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
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == newAssignment.UserId);

            if (user is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"No user with id {newAssignment.UserId} found";
                return serviceResponse;
            }

            //Creation of the new assignment
            Assignment assignment = _mapper.Map<Assignment>(newAssignment);
            assignment.AssignedToUser = user.Username;
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            // Return all assignments as response
            serviceResponse.Data = await _context.Assignments
                .Select(c => _mapper.Map<GetAssignmentDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> DeleteAssignment(int id)
        {
            ServiceResponse<List<GetAssignmentDto>> serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();

            if (id <= 0)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid assignment ID";
                return serviceResponse;
            }
            try
            {
                var assignment = await _context.Assignments.FirstOrDefaultAsync(c => c.Id == id);
                if (assignment == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No assignment with id {id} found.";
                    return serviceResponse;
                }

                _context.Remove(assignment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            //Return a list of the assignments after deletion.
            serviceResponse.Data = await _context.Assignments
                .Select(c => _mapper.Map<GetAssignmentDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetAllAssignments()
        {
            var serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            var dbAssignments = await _context.Assignments.ToListAsync();
            serviceResponse.Data = dbAssignments.Select(c => _mapper.Map<GetAssignmentDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetAssignmentDto>> GetAssignmentById(int id)
        {
            var serviceResponse = new ServiceResponse<GetAssignmentDto>();

            if (id <= 0)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid assignment ID";
                return serviceResponse;
            }
            var dbAssignment = await _context.Assignments.FirstOrDefaultAsync(c => c.Id == id);

            if (dbAssignment == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"No assignment found with ID {id}";
                return serviceResponse;
            }
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

            if (pendingAssignments is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No closed assignments found.";
                return serviceResponse;
            }
            serviceResponse.Data = pendingAssignments;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetOpenAssignments()
        {
            var serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            var dbAssignments = await _context.Assignments.ToListAsync();

            //Checking if there's any assignments in general.
            if (dbAssignments is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No assignments found.";
                return serviceResponse;
            }
            var openAssignments = dbAssignments
                .Where(a => a.Status != Status.Done)
                .Select(a => _mapper.Map<GetAssignmentDto>(a))
                .ToList();

            //Checking if there's any open assignments
            if (openAssignments is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No open assignments found.";
                return serviceResponse;
            }
            serviceResponse.Data = openAssignments;
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

            //Check if there's any assignments due in the upcoming week.
            if (assignmentsDueThisWeek is null || !assignmentsDueThisWeek.Any())
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No assignments due this week";
                return serviceResponse;
            }
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
            ServiceResponse<GetAssignmentDto> serviceResponse = new ServiceResponse<GetAssignmentDto>();
            var existingAssignment = await _context.Assignments
                .FirstOrDefaultAsync(c => c.Id == id);

            //Check if there's an assignment with the ID provided.
            if (existingAssignment is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"No assignment with id {id} found";
                return serviceResponse;
            }
            if (updateAssignment.UserId <= 0)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid ID";
                return serviceResponse;
            }
            //Check to see if the user exists in the db (Avoiding a very high random id input)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateAssignment.UserId);
            if (user is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"No user with id {updateAssignment.UserId} found";
                return serviceResponse;
            }

            _mapper.Map(updateAssignment, existingAssignment);
            serviceResponse.Data = _mapper.Map<GetAssignmentDto>(existingAssignment);
            await _context.SaveChangesAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<UserAssignmentDto>>> GetUsersWithMostAssignmentsDone(DateTime startDate, DateTime endDate)
        {
            var serviceResponse = new ServiceResponse<List<UserAssignmentDto>>();

            //Taking only the assignments fulfiling the cretria of being in between start and end dates.
            var assignments = await _context.Assignments
                .Where(a => a.Status == Status.Done && a.DateCreated >= startDate && a.DateCreated <= endDate)
                .ToListAsync();

            /*  Grouping the data fulfilling the requirments above ^ to a userID (which is unique).
                Then we gave this group a name using the attribute AssignedToUser (which returns the name of the ID)
                Then we just count the number of assignments for each user and return it by highest value first.
            */
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