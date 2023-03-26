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

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
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
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
        {
            var response = await _authRepository.Login(request.Username, request.Password);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}