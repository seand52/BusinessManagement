using Microsoft.AspNetCore.Mvc;
using BusinessManagementApi.Models;
using System.Data;
using AutoMapper;
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
        public ActionResult<ClientDto> Get(int id)
        {
            Client? client = _clientService.GetClientById(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ClientDto>(client));
        }

        [HttpPost]
        public ActionResult<ClientDto> Create([FromBody] CreateClientDto client)
        {
            try
            {
                var clientEntity = _mapper.Map<Client>(client);
                var isSuccess  = _clientService.CreateClient(clientEntity, ModelState);

                if (!isSuccess)
                {
                    return BadRequest(ModelState);
                }

                return CreatedAtAction(nameof(Get), new { id = clientEntity.Id }, _mapper.Map<ClientDto>(clientEntity));
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes");
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Client> Put(int id, [FromBody] UpdateClientDto? client)
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

                var clientEntity = _mapper.Map<Client>(client);
                _clientService.UpdateClient(clientToUpdate, clientEntity);

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
