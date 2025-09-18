using AutoMapper;
using EatIT.Core.DTOs;
using EatIT.Core.Entities;
using EatIT.Core.Interface;
using EatIT.Infrastructure.Data.DTOs;
using EatIT.WebAPI.Errors;
using EatIT.WebAPI.MyHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace EatIT.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RestaurantController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _unitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        [HttpGet("get-all-restaurants")]
        public async Task<ActionResult> GetAllUser([FromQuery] string? search = null)
        {
            try
            {
                var res = await _unitOfWork.RestaurantRepository.GetAllAsync(new Core.Sharing.RestaurantParams 
                    { 
                        Search = search 
                    }
                );

                var totalIteams = await _unitOfWork.RestaurantRepository.CountAsync();
                var result = _mapper.Map<List<RestaurantDTO>>(res);
                return Ok(new { totalIteams, result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseCommentResponse(500, "Đã xảy ra lỗi máy chủ nội bộ khi đang tìm kiếm nhà hàng"));
            }
        }

        [HttpGet("get-restaurant-by-id/{id}")]
        [ResponseType(StatusCodes.Status200OK)]
        [ResponseType(typeof(BaseCommentResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetRestaurantById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new BaseCommentResponse(400, "ID nhà hàng không hợp lệ"));

                var res = await _unitOfWork.RestaurantRepository.GetByIdAsync(id, x => x.Tag);
                if (res == null)
                    return NotFound(new BaseCommentResponse(404, "Không tìm thấy nhà hàng"));

                var result = _mapper.Map<RestaurantDTO>(res);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseCommentResponse(500, "Đã xảy ra lỗi máy chủ nội bộ khi đang tìm kiếm nhà hàng"));
            }
        }
    }
}
