using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HomeAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        //temp mock user
        private static User user = new User{FirstName = "Test", LastName ="Lastname", Email = "email"};

        //IActionResult allows to send HTTP status codes back to the client with the data
        [HttpGet]
        public ActionResult<User> Get() {
            return Ok(user);
        }
    }
}