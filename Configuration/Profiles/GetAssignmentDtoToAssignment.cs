using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.Assignment;

namespace HomeAssignment.Configuration.Profiles
{
    public class GetAssignmentDtoToAssignment : Profile
    {
        public GetAssignmentDtoToAssignment()
        {
            CreateMap<GetAssignmentDto, Assignment>();
        }
    }
}