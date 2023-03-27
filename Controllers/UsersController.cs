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
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> Get()
        {
            _logger.LogInformation("Get users was invoked");
            var response = await _userService.GetAllUsers();
            _logger.LogInformation("Get users completed");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetAssignmentById(int id)
        {
             _logger.LogInformation("Get user by id was invoked");
            var response = await _userService.GetUserById(id);
             _logger.LogInformation("Get user by id completed");
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateUser(UpdateUserDto updatedAssignment, int id)
        {
            _logger.LogInformation("Update user was invoked");
            var response = await _userService.UpdateUser(updatedAssignment, id);
            _logger.LogInformation("Update user completd");
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}