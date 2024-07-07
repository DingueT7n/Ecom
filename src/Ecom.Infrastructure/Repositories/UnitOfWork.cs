using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
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
       
        private readonly IFileProvider fileProvider;

        public UnitOfWork(ApplicationDbContext context , IFileProvider fileProvider)
        {
            _context = context;
           
            this.fileProvider = fileProvider;
            CategoryRepository =new CategoryRepository(context);
            ProductRepository =new ProductRepository(context,fileProvider);

        }

        public ICategoryRepository CategoryRepository {  get;  }

        public IProductRepository ProductRepository { get; }
    }
}
