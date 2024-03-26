using BusinessManagementApi.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BusinessManagement.Templates;


public class PriceData {
    public decimal TotalPrice { get; set; }
    public decimal Tax { get; set; }
    public decimal Re { get; set; }
    
    public PaymentType PaymentMethod { get; set; }
    
    public decimal Transport { get; set; }
    public List<InvoiceProduct> Items { get; set; }
}
public class GrandTotalComponent : IComponent
{
    private PriceData Data { get; }

    public GrandTotalComponent(PriceData data)
    {
        Data = data;
    }

    // TODO: extract this logic somewhere else
    private decimal CalculateTotalPriceOfProducts()
    {
        return Data.Items.Sum(x => x.Price * x.Quantity * (1 - x.Discount));
    }
    
    private decimal CalculateTax()
    {
        return CalculateTotalPriceOfProducts() * Data.Tax;
    }
    
    private decimal CalculateRe()
    {
        return CalculateTotalPriceOfProducts() * Data.Re;
    }
    
    private decimal CalculateSubTotal()
    {
        return CalculateRe() + CalculateTax() + CalculateTotalPriceOfProducts();
    }
    
    private decimal CalculateTransport()
    {
        return Data.Transport;
    }
    
    private decimal CalculateGrandTotal()
    {
        return CalculateSubTotal() + CalculateTransport();
    }
    
    
    public void Compose(IContainer container)
    {
        container.PaddingVertical(25).Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });
            
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("FORMA DE PAGO").FontSize(10);
                header.Cell().Element(CellStyle).AlignRight().Text("BASE IMPONIBLE").FontSize(10);
                header.Cell().Element(CellStyle).AlignRight().Text("IVA (21%)").FontSize(10);
                header.Cell().Element(CellStyle).AlignRight().Text("RE (5,2%)").FontSize(10);
                header.Cell().Element(CellStyle).AlignRight().Text("SUBTOTAL").FontSize(10);
                header.Cell().Element(CellStyle).AlignRight().Text("TRANSPORTE").FontSize(10);
                header.Cell().Element(CellStyle).AlignRight().Text("TOTAL").FontSize(10);
                
                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });
            
            table.Cell().Element(ItemCellStyle).Text(Data.PaymentMethod);
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{CalculateTotalPriceOfProducts()}$");
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{CalculateTax()}$");
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{CalculateRe()}$");
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{CalculateSubTotal()}$");
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{CalculateTransport()}$");
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{CalculateGrandTotal()}$");
            
            static IContainer ItemCellStyle(IContainer container)
            {
                return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }
        });
    }
}