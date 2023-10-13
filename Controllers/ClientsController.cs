using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BusinessManagement.Settings;

public class ClientDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public string? Notes { get; set; }
    // public DateTime CreatedAt { get; set; }
    // public DateTime UpdatedAt { get; set; }
}

namespace BusinessManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        public ClientsController() { }

        [HttpGet("{id}")]
        public Task<ClientDto> Get()
        {
            return Task.FromResult<ClientDto>(new ClientDto() { Id = 1, Name = "Test" });
        }
    }
}
