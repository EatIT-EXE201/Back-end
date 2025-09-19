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

        [HttpPost("add-new-restaurnat")]
        public async Task<ActionResult> AddNewRestaurant([FromForm] CreateRestaurantDTO createRestaurantDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new BaseCommentResponse(400, "Dữ liệu đầu vào không hợp lệ"));
                if (createRestaurantDTO == null)
                    return BadRequest(new BaseCommentResponse(400, "Dữ liệu nhà hàng là bắt buộc"));
                var ok = await _unitOfWork.RestaurantRepository.AddAsync(createRestaurantDTO);
                if (!ok)
                    return BadRequest(new BaseCommentResponse(400, "Không thêm được nhà hàng. Tải ảnh lên không thành công hoặc tạo nhà hàng không thành công."));

                return Ok(ok);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseCommentResponse(500, "Đã xảy ra lỗi máy chủ nội bộ khi thêm nhà hàng"));
            }
        }

        [HttpPut("update-existing-restsaurant/{id}")]
        public async Task<ActionResult> UpdateRestaurant(int id, [FromForm] UpdateRestaurantDTO updateRestaurantDTO)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new BaseCommentResponse(400, "ID nhà hàng không hợp lệ"));

                if (!ModelState.IsValid)
                    return BadRequest(new BaseCommentResponse(400, "Dữ liệu đầu vào không hợp lệ"));

                if (updateRestaurantDTO == null)
                    return BadRequest(new BaseCommentResponse(400, "Cần cập nhật dữ liệu"));

                var res = await _unitOfWork.RestaurantRepository.UpdateAsync(id, updateRestaurantDTO);
                return res ? Ok(updateRestaurantDTO) : NotFound(new BaseCommentResponse(404, "Không tìm thấy nhà hàng hoặc cập nhật không thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseCommentResponse(500, "Đã xảy ra lỗi máy chủ nội bộ khi cập nhật nhà hàng"));
            }
        }

        [HttpDelete("delete-existing-restaurant/{id}")]
        public async Task<ActionResult> DeleteRestaurant(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new BaseCommentResponse(400, "ID nhà hàng không hợp lệ"));

                var res = await _unitOfWork.RestaurantRepository.DeleteAsync(id);
                return res ? Ok(new { message = "Nhà hàng đã bị xóa thành công", id }) : NotFound(new BaseCommentResponse(404, "Không tìm thấy nhà hàng"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new BaseCommentResponse(500, "Đã xảy ra lỗi máy chủ nội bộ khi xóa nhà hàng"));
            }
        }
    }
}
