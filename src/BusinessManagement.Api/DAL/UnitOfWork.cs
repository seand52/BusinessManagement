using System;
using BusinessManagement.DAL;
using BusinessManagement.Database;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;

namespace ContosoUniversity.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private ClientRepository? _clientRepository;
        private GenericRepository<Product>? _productRepository;
        private GenericRepository<BusinessInfo>? _businessInfoRepository;
        private InvoiceRepository? _invoiceRepository;
        private SalesOrderRepository? _salesOrderRepository;
        
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public IClientRepository ClientRepository
        {
            get
            {

                if (_clientRepository == null)
                {
                    _clientRepository = new ClientRepository(_context);
                }
                return _clientRepository;
            }
        }
        
        public IGenericRepository<Product> ProductRepository
        {
            get
            {

                if (_productRepository == null)
                {
                    _productRepository = new GenericRepository<Product>(_context);
                }
                return _productRepository;
            }
        }
        
        public IGenericRepository<BusinessInfo> BusinessInfoRepository
        {
            get
            {

                if (_businessInfoRepository == null)
                {
                    _businessInfoRepository = new GenericRepository<BusinessInfo>(_context);
                }
                return _businessInfoRepository;
            }
        }
        
        public IInvoiceRepository InvoiceRepository
        {
            get
            {

                if (_invoiceRepository == null)
                {
                    _invoiceRepository = new InvoiceRepository(_context);
                }
                return _invoiceRepository;
            }
        }
        
        public ISalesOrderRepository SalesOrderRepository
        {
            get
            {

                if (_salesOrderRepository == null)
                {
                    _salesOrderRepository = new SalesOrderRepository(_context);
                }
                return _salesOrderRepository;
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}