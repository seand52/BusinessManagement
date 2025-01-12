using BusinessManagementApi.Dto;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BusinessManagement.Templates;

public interface ISalesOrderBuilder
{
    ISalesOrderBuilder Build();
    ISalesOrderBuilder CreateSalesOrder(SalesOrderDetailDto data, BusinessInfoDto businessInfo);
    byte[] GeneratePdf();
}

public class SalesOrderBuilder : ISalesOrderBuilder
{
    private IDocument _document;
    public ISalesOrderBuilder Build()
    {

        return new SalesOrderBuilder();
    }
    
    public ISalesOrderBuilder CreateSalesOrder(SalesOrderDetailDto data, BusinessInfoDto businessInfo)
    {
        _document = new SalesOrderDocument(data, businessInfo);
        return this;
    }

    public byte[] GeneratePdf()
    {
       return _document.GeneratePdf();
    }
}
