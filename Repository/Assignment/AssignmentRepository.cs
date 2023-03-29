using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeAssignment.Data;
using HomeAssignment.Domain;
using HomeAssignment.Domain.Enums;
using HomeAssignment.Dtos.Analytics;
using HomeAssignment.Dtos.Assignment;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Repository.Assignment
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public AssignmentRepository(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Create(CreateAssignmentDto newAssignment, User user)
        {
            var assignment = _mapper.Map<Domain.Assignment>(newAssignment);
            assignment.AssignedToUser = user.Username;
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Task<GetAssignmentDto> assignment)
        {
            _context.Remove(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetAssignmentDto>> GetAll()
        {
            return await _context.Assignments
                .Select(c => _mapper.Map<GetAssignmentDto>(c)).ToListAsync();
        }

        public async Task<List<GetAssignmentDto>> GetAssignmentsSortedByDueDate()
        {
            var tasks = await _context.Assignments
                .OrderBy(a => a.DueDate).ToListAsync();
            return tasks.Select(a => _mapper.Map<GetAssignmentDto>(a)).ToList();
        }

        public async Task<List<GetAssignmentDto>> GetAssignmentsSortedByImportance()
        {
            var tasks = await _context.Assignments
                .OrderByDescending(a => a.Importance).ToListAsync();
            return tasks.Select(a => _mapper.Map<GetAssignmentDto>(a)).ToList();
        }

        public async Task<List<GetAssignmentDto>> GetAssignmentsSortedByStatus()
        {
            var tasks = await _context.Assignments
                .OrderByDescending(a => a.Status).ToListAsync();
            return tasks.Select(a => _mapper.Map<GetAssignmentDto>(a)).ToList();
        }

        public async Task<GetAssignmentDto> GetById(int id)
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<GetAssignmentDto>(assignment);
        }

        public async Task<List<GetAssignmentDto>> GetClosed()
        {
            var pendingAssignments = await _context.Assignments
                .Where(a => a.Status == Status.Done)
                .Select(c => _mapper.Map<GetAssignmentDto>(c)).ToListAsync();
            return pendingAssignments;

        }

        public async Task<List<GetAssignmentDto>> GetDueThisWeek(DateTime currentDate, DateTime endDate)
        {
            return await _context.Assignments
                .Where(a => a.DueDate >= currentDate && a.DueDate <= endDate)
                .Select(a => _mapper.Map<GetAssignmentDto>(a))
                .ToListAsync();
        }

        public async Task<List<GetAssignmentDto>> GetOpen()
        {
            return await _context.Assignments
                .Where(a => a.Status != Status.Done)
                .Select(a => _mapper.Map<GetAssignmentDto>(a))
                .ToListAsync();
        }

        // public async Task<List<UserAssignmentDto>> GetUsersWithMostAssignmentsDone(DateTime startDate, DateTime endDate)
        // {
        //     throw new NotImplementedException();
        // }

        public async Task Update(UpdateAssignmentDto updateAssignment, int id)
        {
            var assignment = GetById(id);
            var user = _mapper.Map<Domain.Assignment>(assignment);
            user.Id = assignment.Id;
            user.Title = updateAssignment.Title;
            user.Description = updateAssignment.Description;
            user.Status = updateAssignment.Status;
            user.Importance = updateAssignment.Importance;
            user.UserId = updateAssignment.UserId;
            user.DueDate = updateAssignment.DueDate;
            // var assignment = _mapper.Map(updateAssignment, user);
            // _mapper.Map<GetAssignmentDto>(user);
            
            _context.Assignments.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}