using BusinessManagementApi.Dto;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BusinessManagement.Templates;

public interface IInvoiceDocumentBuilder
{
    IInvoiceDocumentBuilder Build();
    IInvoiceDocumentBuilder CreateInvoiceDocument(InvoiceDetailDto data, BusinessInfoDto businessInfo);
    byte[] GeneratePdf();
}

public class InvoiceDocumentBuilder : IInvoiceDocumentBuilder
{
    private IDocument _document;
    public IInvoiceDocumentBuilder Build()
    {

        return new InvoiceDocumentBuilder();
    }
    
    public IInvoiceDocumentBuilder CreateInvoiceDocument(InvoiceDetailDto data, BusinessInfoDto businessInfo)
    {
        _document = new InvoiceDocument(data, businessInfo);
        return this;
    }

    public byte[] GeneratePdf()
    {
        return _document.GeneratePdf();
    }
}
