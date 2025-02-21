using BusinessManagementApi.Models;

namespace BusinessManagementApi.Extensions.Templates;

public class BasePresenter
{
    public decimal RoundNumber(decimal number)
    {
        return Math.Round(number, 2, MidpointRounding.AwayFromZero);
    }

    public string PaymentTypeMapper(string paymentType)
    {
        if (!Enum.TryParse<PaymentType>(paymentType, out var type)) return "";
        return type switch
        {
            PaymentType.Transfer => "Transferencia",
            PaymentType.Card => "Tarjeta",
            PaymentType.Cash => "Efectivo",
            _ => ""
        };
    }
}