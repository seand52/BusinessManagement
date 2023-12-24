using Microsoft.AspNetCore.Mvc;
using BusinessManagementApi.Models;
using System.Data;
using BusinessManagementApi.Services;

public record ClientDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? ShopName { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? Postcode { get; set; }
    public string? DocumentNum { get; set; }
    public string? Telephone { get; set; }
    public string Email { get; set; }
}

namespace BusinessManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{id}")]
        public ActionResult<ClientDto> Get(int id)
        {
            Client? client = _clientService.GetClientById(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public ActionResult<Client> Create([FromBody] Client client)
        {
            try
            {
                var isSuccess  = _clientService.CreateClient(client, ModelState);

                if (!isSuccess)
                {
                    return BadRequest(ModelState);
                }

                return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes");
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Client> Put(int id, [FromBody] Client? client)
        {
            try
            {
                if (client == null)
                {
                    return BadRequest();
                }

                var clientToUpdate = _clientService.GetClientById(id);

                if (clientToUpdate == null)
                {
                    return NotFound();
                }

                _clientService.UpdateClient(clientToUpdate, client);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Error creating employee"
                );
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                Client? client = _clientService.GetClientById(id);

                if (client == null)
                {
                    return NotFound();
                }

                _clientService.DeleteClient(client);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Unexpected error deleting client"
                );
            }
        }
    }
}
