using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.User;
using HomeAssignment.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace HomeAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> Get()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetAssignmentById(int id)
        {
            var response = await _userService.GetUserById(id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateAssignment(UpdateUserDto updatedAssignment, int id)
        {
            var response = await _userService.UpdateUser(updatedAssignment, id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}