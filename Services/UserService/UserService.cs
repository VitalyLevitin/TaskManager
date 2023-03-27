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
            if (id <= 0)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid assignment ID";
                return serviceResponse;
            }
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == id);

            if (dbUser is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"No user with id {id} found";
                return serviceResponse;
            }
            serviceResponse.Data = _mapper.Map<GetUserDto>(dbUser);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updateUser, int id)
        {
            ServiceResponse<GetUserDto> serviceResponse = new ServiceResponse<GetUserDto>();
            if (id <= 0)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid assignment ID";
                return serviceResponse;
            }
            var existingUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);

            if (existingUser is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"No user with id {id} found";
                return serviceResponse;
            }
            if (!IsValidEmail(updateUser.Email))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid email address format";
                return serviceResponse;
            }
            _mapper.Map(updateUser, existingUser);
            await _context.SaveChangesAsync();
            
            serviceResponse.Data = _mapper.Map<GetUserDto>(existingUser);
            return serviceResponse;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}