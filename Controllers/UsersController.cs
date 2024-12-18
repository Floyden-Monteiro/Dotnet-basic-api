using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BagAPI.Models;
using BagAPI.Services;
using BagAPI.Helper;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    // Constructor: Inject the UserService
    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    // GET: api/users
    [HttpGet]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _userService.GetAllAsync();
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> GetUserById(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
        }

        var response = await _userService.GetByIdAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }

    // POST: api/users
    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> CreateUser([FromBody] Users user)
    {
        if (user == null)
        {
            return BadRequest(new ApiResponse<string>(false, "Invalid user data.", null));
        }

        user.RoleId = "1"; // Assign default role

        var response = await _userService.CreateAsync(user);
        if (response.Success)
        {
            return CreatedAtAction(nameof(GetUserById), new { id = response.Data }, response);
        }

        return BadRequest(response);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] Users updatedUser)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
        }

        var response = await _userService.UpdateAsync(id, updatedUser);
        return response.Success ? Ok(response) : NotFound(response);
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest(new ApiResponse<string>(false, "The 'id' query parameter is required.", null));
        }

        var response = await _userService.DeleteAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }

    // POST: api/users/login
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.email) || string.IsNullOrEmpty(loginRequest.password))
        {
            return BadRequest(new ApiResponse<string>(false, "Email and password are required.", null));
        }

        var token = _userService.Authenticate(loginRequest.email, loginRequest.password);
        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }
}
