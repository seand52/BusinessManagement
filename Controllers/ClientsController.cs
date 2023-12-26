using Microsoft.AspNetCore.Mvc;
using BusinessManagementApi.Models;
using AutoMapper;
using BusinessManagement.Filter;
using BusinessManagementApi.Services;
using BusinessManagementApi.Dto;

namespace BusinessManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientsController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> Get(int id)
        {
            Client? client = await _clientService.GetClientById(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ClientDto>(client));
        }
        
        [HttpGet]
        public  async Task<ActionResult<Client>> GetClients([FromQuery] PaginationFilter filter, [FromQuery] string? SearchTerm)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var clients = await _clientService.GetClients(validFilter, SearchTerm);
            return Ok(clients);
        }

        [HttpPost]
        public async Task<ActionResult<ClientDto>> Create([FromBody] CreateClientDto client)
        {
            var clientEntity = _mapper.Map<Client>(client);
            var isSuccess = await _clientService.CreateClient(clientEntity, ModelState);

            if (!isSuccess)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Get), new { id = clientEntity.Id }, _mapper.Map<ClientDto>(clientEntity));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Client>> Put(int id, [FromBody] UpdateClientDto? client)
        {
            if (client == null)
            {
                return BadRequest();
            }

            var clientToUpdate = await _clientService.GetClientById(id);

            if (clientToUpdate == null)
            {
                return NotFound();
            }

            var clientEntity = _mapper.Map<Client>(client);
            await _clientService.UpdateClient(clientToUpdate, clientEntity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Client? client = await _clientService.GetClientById(id);

            if (client == null)
            {
                return NotFound();
            }

            await _clientService.DeleteClient(client);
            return Ok();
        }
    }
}