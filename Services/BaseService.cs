using BagAPI.Helper;
using BagAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BagAPI.Services
{
    public class BaseService<T> where T : class
    {
        protected readonly BagDBContext _context;

        public BaseService(BagDBContext context)
        {
            _context = context;
        }

        // Create
        public async Task<ApiResponse<T>> CreateAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return new ApiResponse<T>(true, "Entity created successfully", entity);
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>(false, $"Error: {ex.Message}", null);
            }
        }

        // Get All
        public async Task<ApiResponse<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var entities = await _context.Set<T>().ToListAsync();
                return new ApiResponse<IEnumerable<T>>(true, "Entities retrieved successfully", entities);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<T>>(false, $"Error: {ex.Message}", null);
            }
        }

        // Get by Id
        public async Task<ApiResponse<T>> GetByIdAsync(string id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    return new ApiResponse<T>(false, "Entity not found", null);
                }
                return new ApiResponse<T>(true, "Entity retrieved successfully", entity);
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>(false, $"Error: {ex.Message}", null);
            }
        }

        // Update
        public async Task<ApiResponse<T>> UpdateAsync(string id, T entity)
        {
            try
            {
                var existingEntity = await _context.Set<T>().FindAsync(id);
                if (existingEntity == null)
                {
                    return new ApiResponse<T>(false, "Entity not found", null);
                }

                var entityEntry = _context.Entry(existingEntity);

                foreach (var property in entityEntry.Properties)
                {
                    if (!property.Metadata.IsPrimaryKey())
                    {
                        property.CurrentValue = _context.Entry(entity).Property(property.Metadata.Name).CurrentValue;
                    }
                }

                await _context.SaveChangesAsync();
                return new ApiResponse<T>(true, "Entity updated successfully", existingEntity);
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>(false, $"Error: {ex.Message}", null);
            }
        }

        // Delete
        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    return new ApiResponse<bool>(false, "Entity not found", false);
                }

                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true, "Entity deleted successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
        }
    }
}
