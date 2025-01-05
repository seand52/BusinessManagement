using BusinessManagementApi.Models;

namespace BusinessManagement.UnitTests.ModhelExtensions
{
    [TestFixture]
    public class ModelExtensions
    {
        [Test]
        public void ModelExtensions_CalculateTotalPrice_ReturnsCorrectPrice()
        {
            // create a fixture of an invoice with some invoices products defined by me
            var fixture = new Fixture();
            var invoice = fixture.Build<Invoice>()
                .With(x => x.InvoiceProducts, new List<InvoiceProduct>
                {
                    new() {ProductId = 1, Quantity = 2, Price = 20, Discount = 0.4m },
                    new() {ProductId = 2, Quantity = 1, Price = 30, Discount = 0m},
                    new() {ProductId = 3, Quantity = 3, Price = 50, Discount = 0.1m}
                })
                .With(x => x.Tax, 0.2m)
                .With(x  => x.Re, 0.05m)
                .With(x => x.TransportPrice, 10m )
                .Create();
            var totalPrice = invoice.CalculateTotalPrice();
            Assert.That(totalPrice, Is.EqualTo(246.25m));
        }
    }
}