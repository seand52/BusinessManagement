using Microsoft.AspNetCore.Mvc;
using BusinessManagementApi.Models;
using AutoMapper;
using BusinessManagement.Commands;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace BusinessManagement.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BusinessInfoController : BusinessManagementController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BusinessInfoController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BusinessInfoDto>> Get()
        {
            var result = await _mediator.Send(new GetBusinessInfoQuery(GetUserId()));
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<BusinessInfoDto>> Create([FromBody] CreateBusinessInfoDto businessInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mediator.Send(new CreateBusinessInfoRequest(businessInfo, GetUserId()));
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut]
        public async Task<ActionResult<BusinessInfo>> Put([FromBody] UpdateBusinessInfoDto? businessInfo)
        {
            if (businessInfo == null)
            {
                return BadRequest();
            }
            
            var success = await _mediator.Send(new UpdateBusinessInfoRequest(businessInfo, GetUserId()));
            
            return success ? NoContent() : BadRequest();
        }
    }
}