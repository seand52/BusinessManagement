using BusinessManagement.Templates;
using BusinessManagementApi.Models;

namespace BusinessManagement.Helpers;

public class Calculator
{
    private PriceData Data { get; set; }
    private readonly decimal _totalPriceOfProducts;
    
    public Calculator(PriceData priceData)
    {
        Data = priceData;
        _totalPriceOfProducts = CalculateTotalPriceOfProducts();
    }
    
    public string PaymentMethod => Data.PaymentMethod;
    
    public decimal CalculateTotalPriceOfProducts()
    {
        return Data.Items.Sum(x => x.Price * x.Quantity * (1 - x.Discount));
    }
    
    public decimal CalculateTax()
    {
        return _totalPriceOfProducts * Data.Tax;
    }
    
    public decimal CalculateRe()
    {
        return _totalPriceOfProducts * Data.Re;
    }
    
    public decimal CalculateSubTotal()
    {
        return CalculateRe() + CalculateTax() + _totalPriceOfProducts;
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