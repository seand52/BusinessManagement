namespace BusinessManagementApi.Models;

public class CalculableItem: ICalculableItem
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
}