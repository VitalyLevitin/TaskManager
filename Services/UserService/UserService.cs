using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeAssignment.Data;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.User;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResponse<List<GetUserDto>>> GetAllUsers()
        {
            var response = new ServiceResponse<List<GetUserDto>>();
            var dbUsers = await _context.Users.ToListAsync();
            response.Data = dbUsers.Select(c => _mapper.Map<GetUserDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();
            var dbUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetUserDto>(dbUser);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updateUser, int id)
        {
            ServiceResponse<GetUserDto> response = new ServiceResponse<GetUserDto>();
            User? existingUser = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == id);
            if (existingUser is null)
            {
                response.Success = false;
                response.Message = $"No user with id {id} found";
                return response;
            }

            _mapper.Map(updateUser, existingUser);
            response.Data = _mapper.Map<GetUserDto>(existingUser);
            await _context.SaveChangesAsync();
            return response;
        }
    }
}