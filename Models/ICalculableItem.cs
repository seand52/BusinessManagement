namespace BusinessManagementApi.Models;

public interface ICalculableItem
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
}

public interface IPriceCalculable
{
    IEnumerable<ICalculableItem> Items { get; }
    decimal TransportPrice { get; }
    decimal Tax { get; }
    decimal Re { get; }
}