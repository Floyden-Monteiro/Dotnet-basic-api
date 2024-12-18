using BagAPI.Data;
using BagAPI.Models;

namespace BagAPI.Services
{
    public class ProductService : BaseService<Products>
    {
        public ProductService(BagDBContext context) : base(context) { }
    }
}
