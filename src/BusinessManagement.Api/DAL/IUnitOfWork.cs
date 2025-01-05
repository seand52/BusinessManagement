using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;

namespace BusinessManagement.DAL;

public interface IUnitOfWork: IDisposable
{
    public IGenericRepository<Client> ClientRepository { get; }
    public IGenericRepository<Product> ProductRepository { get; }
    public IGenericRepository<BusinessInfo> BusinessInfoRepository { get; }
    public IInvoiceRepository InvoiceRepository { get; }
    public ISalesOrderRepository SalesOrderRepository { get; }
    public Task Save();
}