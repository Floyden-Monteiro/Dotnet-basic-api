using BagAPI.Helper;
using BagAPI.Models;
using BagAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BagAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly UserService _userService;

        public RolesController(RoleService roleService, UserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        // GET: api/roles
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var response = await _roleService.GetAllAsync();
            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        // GET: api/roles/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));

            var response = await _roleService.GetByIdAsync(id);
            if (response.Success)
                return Ok(response);

            return NotFound(response);
        }

        // GET: api/roles/{id}/users
        [HttpGet("{id}/users")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetUsersByRole(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));

            var roleResponse = await _roleService.GetByIdAsync(id);
            if (!roleResponse.Success)
                return NotFound(new ApiResponse<string>(false, "Role not found.", null));

            var usersResponse = await _userService.GetUsersByRoleIdAsync(id);
            if (usersResponse.Success)
                return Ok(usersResponse);

            return NotFound(usersResponse);
        }

        // POST: api/roles
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] Roles role)
        {
            if (role == null)
                return BadRequest(new ApiResponse<string>(false, "Invalid role data.", null));

            var response = await _roleService.CreateAsync(role);
            if (response.Success)
                return CreatedAtAction(nameof(GetById), new { id = response.Data }, response);

            return BadRequest(response);
        }

        // PUT: api/roles/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update(string id, [FromBody] Roles updatedRole)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));

            var response = await _roleService.UpdateAsync(id, updatedRole);
            if (response.Success)
                return Ok(response);

            return NotFound(response);
        }

        // DELETE: api/roles
        [HttpDelete]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete([FromBody] DeleteRoleRequest request)
        {
            if (string.IsNullOrEmpty(request?.id))
                return BadRequest(new ApiResponse<string>(false, "The 'id' body parameter is required.", null));

            var response = await _roleService.DeleteAsync(request.id);
            if (response.Success)
                return Ok(response);

            return NotFound(response);
        }
    }
}
