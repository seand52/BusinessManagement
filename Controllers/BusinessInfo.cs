using Microsoft.AspNetCore.Mvc;
using BusinessManagementApi.Models;
using AutoMapper;
using BusinessManagementApi.Services;
using BusinessManagementApi.Dto;

namespace BusinessManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessInfoController : BusinessManagementController
    {
        private readonly IBusinessInfoService _businessInfoService;
        private readonly IMapper _mapper;

        public BusinessInfoController(IBusinessInfoService businessInfoService, IMapper mapper)
        {
            _businessInfoService = businessInfoService;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<BusinessInfoDto>> Get(int userId)
        {
            BusinessInfo? businessInfo = await _businessInfoService.GetBusinessInfoByUserId(userId);
            if (businessInfo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BusinessInfoDto>(businessInfo));
        }

        [HttpPost]
        public async Task<ActionResult<BusinessInfoDto>> Create([FromBody] CreateBusinessInfoDto businessInfo)
        {
            var businessInfoEntity = _mapper.Map<BusinessInfo>(businessInfo);
            var isSuccess = await _businessInfoService.CreateBusinessInfo(businessInfoEntity, ModelState);

            if (!isSuccess)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Get), new { id = businessInfoEntity.Id }, _mapper.Map<BusinessInfoDto>(businessInfoEntity));
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<BusinessInfo>> Put(int userId, [FromBody] UpdateBusinessInfoDto? businessInfo)
        {
            if (businessInfo == null)
            {
                return BadRequest();
            }

            var businessInfoToUpdate = await _businessInfoService.GetBusinessInfoByUserId(userId);

            if (businessInfoToUpdate == null)
            {
                return NotFound();
            }

            var businessInfoEntity = _mapper.Map<BusinessInfo>(businessInfo);
            await _businessInfoService.UpdateBusinessInfo(businessInfoToUpdate, businessInfoEntity);

            return NoContent();
        }
    }
}