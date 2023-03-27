using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Domain;
using HomeAssignment.Dtos.Assignment;
using HomeAssignment.Services.AssignmentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly ILogger<AssignmentsController> _logger;

        public AssignmentsController(IAssignmentService assignmentService, ILogger<AssignmentsController> logger)
        {
            _assignmentService = assignmentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> Get()
        {
            _logger.LogInformation("Get all assignments was invoked");
            var response = await _assignmentService.GetAllAssignments();
            _logger.LogInformation("Get all assignments completed.");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetAssignmentDto>>> GetAssignmentById(int id)
        {
            _logger.LogInformation("Get assignment by ID was invoked");
            var response = await _assignmentService.GetAssignmentById(id);
            _logger.LogInformation("Get assignment by ID completed.");
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> CreateAssignment(CreateAssignmentDto newAssignment)
        {
            _logger.LogInformation("Create assignment was invoked");
            var response = await _assignmentService.CreateAssignment(newAssignment);
            _logger.LogInformation("Create assignment completed");
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetAssignmentDto>>> UpdateAssignment(UpdateAssignmentDto updatedAssignment, int id)
        {
            _logger.LogInformation("Update assignment was invoked");
            var response = await _assignmentService.UpdateAssignment(updatedAssignment, id);
            _logger.LogInformation("Update assignment completed");
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> Delete(int id)
        {
            _logger.LogInformation("Delete assignment was invoked");
            var response = await _assignmentService.DeleteAssignment(id);
            _logger.LogInformation("Delete assignment completed");
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("Status/Done")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> GetClosedAssignments()
        {
            _logger.LogInformation("Get closed assignments was invoked");
            var response = await _assignmentService.GetClosedAssignments();
            _logger.LogInformation("Get closed assignment completed");
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("Status/Open")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> GetOpenAssignments()
        {
            _logger.LogInformation("Get open assignments was invoked");
            var response = await _assignmentService.GetOpenAssignments();
            _logger.LogInformation("Get open assignment completed");
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("DueThisWeek")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> GetAssignmentsDueThisWeek()
        {
            _logger.LogInformation("Get assignments due this week was invoked");
            var response = await _assignmentService.GetAssignmentsDueThisWeek();
            _logger.LogInformation("Get assignments due this week completed");
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("SortBy/{type}")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> GetAssignmentsSortedBy(string type)
        {
            _logger.LogInformation("Get assignments sorted by was invoked");
            var response = await _assignmentService.GetAssignmentsSortedBy(type);
            _logger.LogInformation("Get assignment sorted by completed");
            return Ok(response);
        }

        [HttpGet("Analytics/TopUser")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> GetUsersWithMostAssignmentsDone(DateTime start, DateTime end)
        {
            _logger.LogInformation("Get user with most assignments was invoked");
            var response = await _assignmentService.GetUsersWithMostAssignmentsDone(start, end);
            _logger.LogInformation("Get user with most assignments completed");
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }



    }
}