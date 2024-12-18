using System.Text.Json.Serialization;

namespace BagAPI.Models;

public class Users
{
    public string? id { get; set; }  // Primary Key
    public string? name { get; set; }
    public string? email { get; set; }
    public string? gender { get; set; }

    public string? password { get; set; }


    public string? RoleId { get; set; }


    [JsonIgnore]
    public Roles? role { get; set; }

   
}
