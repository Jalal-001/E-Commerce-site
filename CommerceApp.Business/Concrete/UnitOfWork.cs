﻿using CommerceApp.Business.Abstract;
using CommerceApp.Data.Abstract;
using CommerceApp.Data.Concrete.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Business.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CommerceAppContext _context;
        public UnitOfWork(CommerceAppContext context)
        {
            _context = context;
        }

        private EfCoreCartRepository _cartRepository;
        private EfCoreCategoryRepository _categoryRepository;
        private EfCoreOrderRepository _orderRepository;
        private EfCoreProductRepository _productRepository;

        public ICartRepository Carts =>
            _cartRepository = _cartRepository ?? new EfCoreCartRepository(_context);

        public ICategoryRepository Categories =>
            _categoryRepository = _categoryRepository ?? new EfCoreCategoryRepository(_context);

        public IOrderRepository Orders =>
            _orderRepository = _orderRepository ?? new EfCoreOrderRepository(_context);
        public IProductRepository Products => 
            _productRepository=_productRepository?? new EfCoreProductRepository(_context);  

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
