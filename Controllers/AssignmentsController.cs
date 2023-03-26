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
            return Ok(await _assignmentService.GetAssignmentById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetAssignmentDto>>>> CreateAssignment(CreateAssignmentDto newAssignment)
        {
            return Ok(await _assignmentService.CreateAssignment(newAssignment));
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
    }
}