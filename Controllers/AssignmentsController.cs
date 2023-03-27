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

        public AssignmentsController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> Get()
        {
            return Ok(await _assignmentService.GetAllAssignments());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetAssignmentDto>>> GetAssignmentById(int id)
        {
            var response = await _assignmentService.GetAssignmentById(id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> CreateAssignment(CreateAssignmentDto newAssignment)
        {
            var response = await _assignmentService.CreateAssignment(newAssignment);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetAssignmentDto>>> UpdateAssignment(UpdateAssignmentDto updatedAssignment, int id)
        {
            var response = await _assignmentService.UpdateAssignment(updatedAssignment, id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> Delete(int id)
        {
            var response = await _assignmentService.DeleteAssignment(id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("Status/Done")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> GetClosedAssignments()
        {
            var response = await _assignmentService.GetClosedAssignments();
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("Status/Open")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> GetOpenAssignments()
        {
            var response = await _assignmentService.GetOpenAssignments();
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("DueThisWeek")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> GetAssignmentsDueThisWeek()
        {
            var response = await _assignmentService.GetAssignmentsDueThisWeek();
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("SortBy/{type}")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> GetAssignmentsSortedBy(string type)
        {
            return Ok(await _assignmentService.GetAssignmentsSortedBy(type));
        }

        [HttpGet("Analytics/TopUser")]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> GetUsersWithMostAssignmentsDone(DateTime start, DateTime end)
        {
            var response = await _assignmentService.GetUsersWithMostAssignmentsDone(start, end);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }



    }
}