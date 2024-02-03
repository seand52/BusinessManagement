using Microsoft.AspNetCore.Mvc;
using BusinessManagementApi.Models;
using AutoMapper;
using BusinessManagementApi.Services;
using BusinessManagementApi.Dto;
using Microsoft.AspNetCore.Authorization;

namespace BusinessManagement.Controllers
{
    [ApiController]
    [Authorize]
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

        [HttpGet]
        public async Task<ActionResult<BusinessInfoDto>> Get()
        {
            BusinessInfo? businessInfo = await _businessInfoService.GetBusinessInfoByUserId(GetUserId());
            if (businessInfo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BusinessInfoDto>(businessInfo));
        }

        [HttpPost]
        public async Task<ActionResult<BusinessInfoDto>> Create([FromBody] CreateBusinessInfoDto businessInfo)
        {
            var existingBusinessInfo = await _businessInfoService.GetBusinessInfoByUserId(GetUserId());

            if (existingBusinessInfo != null)
            {
                return Forbid("Only allowed to have one business associated");
            }
            var businessInfoEntity = _mapper.Map<BusinessInfo>(businessInfo);
            businessInfoEntity.UserId = GetUserId();
            var isSuccess = await _businessInfoService.CreateBusinessInfo(businessInfoEntity, ModelState);

            if (!isSuccess)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Get), new { id = businessInfoEntity.Id }, _mapper.Map<BusinessInfoDto>(businessInfoEntity));
        }

        [HttpPut]
        public async Task<ActionResult<BusinessInfo>> Put([FromBody] UpdateBusinessInfoDto? businessInfo)
        {
            if (businessInfo == null)
            {
                return BadRequest();
            }

            var businessInfoToUpdate = await _businessInfoService.GetBusinessInfoByUserId(GetUserId());

            if (businessInfoToUpdate == null)
            {
                return NotFound();
            }

            if (businessInfoToUpdate.UserId != GetUserId())
            {
                return Unauthorized("Insufficient Permissions");
            }

            var businessInfoEntity = _mapper.Map<BusinessInfo>(businessInfo);
            await _businessInfoService.UpdateBusinessInfo(businessInfoToUpdate, businessInfoEntity);

            return NoContent();
        }
    }
}