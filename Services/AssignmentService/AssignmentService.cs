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
using HomeAssignment.Repository.Assignment;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Services.AssignmentService
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentService(IMapper mapper, DataContext context, IAssignmentRepository assignmentRepository)
        {
            _mapper = mapper;
            _context = context;
            _assignmentRepository = assignmentRepository;
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
            await _assignmentRepository.Create(newAssignment, user);

            // Return all assignments as response
            serviceResponse.Data = await _assignmentRepository.GetAll();
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

            var assignment = _assignmentRepository.GetById(id);
            if (assignment == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"No assignment with id {id} found.";
                return serviceResponse;
            }

            await _assignmentRepository.Delete(assignment);

            //Return a list of the assignments after deletion.
            serviceResponse.Data = await _assignmentRepository.GetAll();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetAllAssignments()
        {
            var serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            serviceResponse.Data = await _assignmentRepository.GetAll();
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
            var dbAssignment = await _assignmentRepository.GetById(id);

            if (dbAssignment == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"No assignment found with ID {id}";
                return serviceResponse;
            }
            serviceResponse.Data = dbAssignment;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetClosedAssignments()
        {
            var serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            var pendingAssignments = await _assignmentRepository.GetClosed();

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
            var openAssignments = await _assignmentRepository.GetOpen(); //Status != Done

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
            var assignmentsDueThisWeek = await _assignmentRepository.GetDueThisWeek(currentDate, endDate);

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

            var dbAssignments = await _assignmentRepository.GetAll();
            // Sort assignments by the specified property
            switch (sortBy.ToLower())
            {
                case "date":
                    dbAssignments = await _assignmentRepository.GetAssignmentsSortedByDueDate();
                    break;
                case "status":
                    dbAssignments = await _assignmentRepository.GetAssignmentsSortedByStatus();
                    break;
                case "importance":
                    dbAssignments = await _assignmentRepository.GetAssignmentsSortedByImportance();
                    break;
            }

            serviceResponse.Data = dbAssignments;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetAssignmentDto>> UpdateAssignment(UpdateAssignmentDto updateAssignment, int id)
        {
            ServiceResponse<GetAssignmentDto> serviceResponse = new ServiceResponse<GetAssignmentDto>();

            //Check if there's an assignment with the ID provided.
            // if (existingAssignment is null)
            // {
            //     serviceResponse.Success = false;
            //     serviceResponse.Message = $"No assignment with id {id} found";
            //     return serviceResponse;
            // }
            if (updateAssignment.UserId <= 0)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid user ID";
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
            serviceResponse.Success = true;
            serviceResponse.Message = $"Updated user with id {updateAssignment.UserId}";
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