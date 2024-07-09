using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration config;

        public UnitOfWork(ApplicationDbContext context , IFileProvider fileProvider, IConfiguration config)
        {
            _context = context;
           
            this.fileProvider = fileProvider;
            this.config = config;
            CategoryRepository =new CategoryRepository(context);
            ProductRepository =new ProductRepository(context,fileProvider,config);

        }

        public ICategoryRepository CategoryRepository {  get;  }

        public IProductRepository ProductRepository { get; }
    }
}
