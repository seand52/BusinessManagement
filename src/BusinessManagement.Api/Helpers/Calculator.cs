using BusinessManagement.Templates;
using BusinessManagementApi.Models;

namespace BusinessManagement.Helpers;

public class Calculator
{
    private PriceData Data { get; set; }
    
    public Calculator(PriceData priceData)
    {
        Data = priceData;
    }
    
    public string PaymentMethod => Data.PaymentMethod;
    
    public decimal CalculateTotalPriceOfProducts()
    {
        return Data.Items.Sum(x => x.Price * x.Quantity * (1 - x.Discount));
    }
    
    public decimal CalculateTax()
    {
        return CalculateTotalPriceOfProducts() * Data.Tax;
    }
    
    public decimal CalculateRe()
    {
        return CalculateTotalPriceOfProducts() * Data.Re;
    }
    
    public decimal CalculateSubTotal()
    {
        return CalculateRe() + CalculateTax() + CalculateTotalPriceOfProducts();
    }
    
    public decimal CalculateTransport()
    {
        return Data.Transport;
    }
    
    public decimal CalculateGrandTotal()
    {
        return CalculateSubTotal() + CalculateTransport();
    }
}