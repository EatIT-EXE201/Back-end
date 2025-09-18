using EatIT.Core.DTOs;
using EatIT.Core.Interface;
using EatIT.Infrastructure.Data;
using EatIT.WebAPI.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EatIT.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly ITokenService _tokenService;
        public AuthController(ApplicationDBContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new BaseCommentResponse(400, "Dữ liệu đầu vào không hợp lệ"));

                if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                    return BadRequest(new BaseCommentResponse(400, "Email và mật khẩu là bắt buộc"));

                var user = await _db.Users.Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);

                if (user == null)
                    return Unauthorized(new BaseCommentResponse(401, "Thông tin đăng nhập không hợp lệ"));

                var token = _tokenService.CreateToken(user, user.Role?.RoleName ?? string.Empty);

                return Ok(new
                {
                    token,
                    user = new { user.Id, user.UserName, user.Email, user.RoleId, RoleName = user.Role?.RoleName }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseCommentResponse(500, "Đã xảy ra lỗi máy chủ nội bộ trong quá trình đăng nhập"));
            }
        }
    }
}
