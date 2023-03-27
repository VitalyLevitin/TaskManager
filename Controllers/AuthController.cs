using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Data;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HomeAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthRepository authRepository, ILogger<AuthController> logger)
        {
            _authRepository = authRepository;
            _logger = logger;
        }

        [HttpPost("register")]

        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            _logger.LogInformation("Register user was invoked");
            var response = new ServiceResponse<int>();
            try
            {
                response = await _authRepository.Register(
                    new User { Username = request.Username, Email = request.Email }, request.Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _logger.LogInformation("Register user completed");
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
        {
            _logger.LogInformation("Login to a specific user was invoked");
            var response = await _authRepository.Login(request.Username, request.Password);
            _logger.LogInformation("Login to a specific user completed");
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}