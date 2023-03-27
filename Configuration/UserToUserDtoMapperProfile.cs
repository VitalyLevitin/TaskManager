using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.User;

namespace HomeAssignment.Configuration
{
    public class UserToUserDtoMapperProfile : Profile
    {
        public UserToUserDtoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
        }
    }
}