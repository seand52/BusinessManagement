using BusinessManagement.Database;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using ContosoUniversity.DAL;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementApi.DAL

{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly ApplicationContext _context;

        public ClientRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }


        public async Task<PagedList<Client>> GetAllBy(string userId, PaginationFilter paginationFilter, string? searchTerm)
        {
            var query = _context.Clients.Where(t => t.UserId == userId);

            if (searchTerm != null)
            {
                query = query.Where(c => c.Name.Contains(searchTerm));
            }
            
            query = query.OrderBy(c => c.Name).AsNoTracking();

            return await PagedList<Client>.CreateAsync(query, paginationFilter.PageNumber, paginationFilter.PageSize);
        }
    }
}