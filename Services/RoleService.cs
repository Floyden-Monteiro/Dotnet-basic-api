using BagAPI.Data;
using BagAPI.Models;

namespace BagAPI.Services
{
    public class RoleService : BaseService<Roles>
    {
        public RoleService(BagDBContext context) : base(context) { }
    }
}
