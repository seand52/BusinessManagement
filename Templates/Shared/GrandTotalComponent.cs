using BusinessManagement.Helpers;
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
    public List<CalculableItem> Items { get; set; }
}
public class GrandTotalComponent : IComponent
{
    private Calculator PriceCalculator { get; }

    public GrandTotalComponent(Calculator calculator)
    {
        PriceCalculator = calculator;
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
            
            table.Cell().Element(ItemCellStyle).Text(PriceCalculator.PaymentMethod);
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{PriceCalculator.CalculateTotalPriceOfProducts()}$");
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{PriceCalculator.CalculateTax()}$");
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{PriceCalculator.CalculateRe()}$");
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{PriceCalculator.CalculateSubTotal()}$");
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{PriceCalculator.CalculateTransport()}$");
            table.Cell().Element(ItemCellStyle).AlignRight().Text($"{PriceCalculator.CalculateGrandTotal()}$");
            
            static IContainer ItemCellStyle(IContainer container)
            {
                return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }
        });
    }
}