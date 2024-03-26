using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BusinessManagement.Templates;

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string? Province { get; set; }
    public string Postcode { get; set; }
    public string? Country { get; set; }
}

public class Person
{
    public string Name { get; set; }
    public string DocumentType { get; set; }
    public string DocumentNumber { get; set; }
}

public class AddressComponent : IComponent
{
    private string Title { get; }
    private Address Address { get; }
    private Person Person { get; }

    public AddressComponent(string title, Address address, Person person)
    {
        Title = title;
        Address = address;
        Person = person;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);

            column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

            column.Item().Text(Person.Name);
            column.Item().Text($"{Person.DocumentType}: {Person.DocumentNumber}");
            column.Item().Text(Address.Street);
            column.Item().Text($"{Address.City}, {Address.Postcode}");
            // TODO: Should be country for both
            column.Item().Text(Address.Province ?? Address.Country);
        });
    }
}