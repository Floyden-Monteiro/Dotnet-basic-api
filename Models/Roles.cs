using System.Text.Json.Serialization;

namespace BagAPI.Models;

public class Roles
{
    public string id { get; set; }  // Primary Key
    public string name { get; set; }

    // Navigation property to access users assigned to this role

    [JsonIgnore]
    public ICollection<Users> Users { get; set; }
}
