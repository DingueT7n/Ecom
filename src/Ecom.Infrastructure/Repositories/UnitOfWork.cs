using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
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
        private readonly IConnectionMultiplexer _redis;
        public UnitOfWork(ApplicationDbContext context , IFileProvider fileProvider, IConfiguration config,IConnectionMultiplexer redis)
        {
            _context = context;
           
            this.fileProvider = fileProvider;
            this.config = config;
            _redis = redis;
            CategoryRepository =new CategoryRepository(context);
            ProductRepository =new ProductRepository(context,fileProvider,config);
            BasketRepository = new BasketRepository(_redis);

        }

        public ICategoryRepository CategoryRepository {  get;  }

        public IProductRepository ProductRepository { get; }

        public IBasketRepository BasketRepository {  get; }
    }
}
