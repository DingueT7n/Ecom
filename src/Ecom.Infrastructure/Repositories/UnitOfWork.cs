using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context ;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository =new CategoryRepository(context);
            ProductRepository =new ProductRepository(context);

        }

        public ICategoryRepository CategoryRepository {  get;  }

        public IProductRepository ProductRepository { get; }
    }
}
