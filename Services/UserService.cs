using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BagAPI.Services;
using BagAPI.Data;
using System.Security.Claims;
using BagAPI.Models;
using Microsoft.EntityFrameworkCore;
using BagAPI.Helper;

public class UserService : BaseService<Users>
{
    private readonly IConfiguration _configuration;

    public UserService(BagDBContext context, IConfiguration configuration) : base(context)
    {
        _configuration = configuration;
    }




    public string Authenticate(string email, string password)
    {
        var user = _context.Users.Include(u => u.role).FirstOrDefault(u => u.email == email && u.password == password);
        if (user == null) return null;

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, user.name),
        new Claim(ClaimTypes.Role, user.role.name) 
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


     public async Task<ApiResponse<List<Users>>> GetUsersByRoleIdAsync(string roleId)
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.RoleId == roleId)
                    .ToListAsync();

                if (users == null || !users.Any())
                {
                    return new ApiResponse<List<Users>>(false, "No users found for the given role.", null);
                }

                return new ApiResponse<List<Users>>(true, "Users fetched successfully.", users);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Users>>(false, ex.Message, null);
            }
        }

}
