using System.Globalization;
using BusinessManagement.Helpers;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Extensions.Templates;
using BusinessManagementApi.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BusinessManagement.Templates;

public class InvoiceDocument : BasePresenter, IDocument
{
    private InvoiceDetailDto Model { get; }
    private BusinessInfoDto BusinessInfo { get; }

    public InvoiceDocument(InvoiceDetailDto model, BusinessInfoDto businessInfo)
    {
        Model = model;
        BusinessInfo = businessInfo;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        var style = TextStyle.Default.FontSize(10);
        container
            .Page(page =>
            {
                page.Margin(50);
                page.DefaultTextStyle(style);
            
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                    
                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }
    
    void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
    
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Factura #{Model.Id}").Style(titleStyle);

                column.Item().Text(text =>
                {
                    text.Span("Fecha de emisión: ").SemiBold();
                    text.Span(Model.DateIssued.ToString("dd/MM/yyyy"));
                });

                if (Model.ExpirationDate != null)
                {
                    column.Item().Text(text =>
                    {
                        text.Span("Fecha de expiración: ").SemiBold();
                        text.Span(Model.ExpirationDate.Value.ToString("dd/MM/yyyy"));
                    });
                }
            });
            
            row.ConstantItem(100).Height(50).Image(Path.GetFullPath("logo.png")).FitHeight();
        });
        
    }
    
    void ComposeContent(IContainer container)
    {
        var clientAddress = new Address()
        {
            Street = Model.Client.Address,
            City = Model.Client.City,
            Postcode = Model.Client.Postcode,
            Province = Model.Client.Province
        };

        var businessAddress = new Address()
        {
            Street = BusinessInfo.Address,
            City = BusinessInfo.City,
            Postcode = BusinessInfo.Postcode,
            Country = BusinessInfo.Country
        };

        var clientInfo = new Person()
        {
            Name = Model.Client.Name,
            DocumentType = Model.Client.DocumentType,
            DocumentNumber = Model.Client.DocumentNum,
        };
        
        var businessInfo = new Person()
        {
            Name = BusinessInfo.Name,
            DocumentType = "Cif",
            DocumentNumber = BusinessInfo.Cif,
        };
        
        container.PaddingVertical(25).Column(column =>
        {
            column.Spacing(5);
            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new AddressComponent("EMISOR", businessAddress, businessInfo ));
                row.ConstantItem(50);
                row.RelativeItem().Component(new AddressComponent("RECEPTOR", clientAddress, clientInfo));
            });
            column.Item().Element(ComposeTable);

            var invoiceProducts = Model.InvoiceProducts.Select(x => new CalculableItem()
            {
                Price = x.Price,
                Quantity = x.Quantity,
                Discount = x.Discount
            }).ToList();

            column.Item().Component(new GrandTotalComponent(new Calculator(new PriceData()
            {
                Items = invoiceProducts,
                Tax = Model.Tax,
                Re = Model.Re,
                Transport = Model.TransportPrice,
                PaymentMethod = Model.PaymentType,
                TotalPrice = Model.TotalPrice
            })));
            
            if (!string.IsNullOrWhiteSpace("some comment"))
                column.Item().PaddingTop(25).Element(ComposeComments);
        });
    }
    

    void ComposeComments(IContainer container)
    {
        container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
        {
            column.Spacing(5);
            column.Item().Text("Comments").FontSize(14);
        });
    }
    void ComposeTable(IContainer container)
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
            });
            
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Referencia").FontSize(10);
                header.Cell().Element(CellStyle).AlignRight().Text("Cantidad").FontSize(10);
                header.Cell().Element(CellStyle).AlignRight().Text("Precio Unidad").FontSize(10);
                header.Cell().Element(CellStyle).AlignRight().Text("Descuento").FontSize(10);
                header.Cell().Element(CellStyle).AlignRight().Text("Total").FontSize(10);
                
                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });
            
            foreach (var item in Model.InvoiceProducts)
            {
                table.Cell().Element(CellStyle).Text(item.Reference);
                table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
                table.Cell().Element(CellStyle).AlignRight().Text($"{RoundNumber(item.Price)}€");
                table.Cell().Element(CellStyle).AlignRight().Text($"{(item.Discount * 100).ToString(CultureInfo.InvariantCulture)}%");
                table.Cell().Element(CellStyle).AlignRight().Text($"{RoundNumber(item.Price * item.Quantity)}€");
                
                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }

}