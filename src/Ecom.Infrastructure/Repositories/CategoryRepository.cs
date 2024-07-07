using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public ApplicationDbContext _context { get; set; }
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //public async Task<Category> GetAsync(int id)
        //{
        //   var response =await  _context.Categories.FindAsync(id);
        //    return response;

        //}
    }
}
