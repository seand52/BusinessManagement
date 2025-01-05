using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;

namespace BusinessManagement.UnitTests.PagedList
{
    [TestFixture]
    public class PagedList
    {
        [Test]
        public async Task PagedList_ValidInputWithNextPage_ReturnsPaginatedData()
        {
            var data = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            var mock = data.BuildMock();
            
            var result = await Helpers.PagedList<string>.CreateAsync(mock, 1, 5);
            Assert.That(result, Is.TypeOf<Helpers.PagedList<string>>());
            Assert.That(result.TotalCount, Is.EqualTo(10));
            Assert.That(result.Items.Count, Is.EqualTo(5));
            Assert.That(result.HasNextPage, Is.True);
            Assert.That(result.HasPreviousPage, Is.False);
        }
        [Test]
        public async Task PagedList_ValidInputWithoutNextPage_ReturnsPaginatedData()
        {
            var data = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            var mock = data.BuildMock();
            
            var result = await Helpers.PagedList<string>.CreateAsync(mock, 1, 15);
            Assert.That(result, Is.TypeOf<Helpers.PagedList<string>>());
            Assert.That(result.TotalCount, Is.EqualTo(10));
            Assert.That(result.Items.Count, Is.EqualTo(10));
            Assert.That(result.HasNextPage, Is.False);
            Assert.That(result.HasPreviousPage, Is.False);
        }
    }
}