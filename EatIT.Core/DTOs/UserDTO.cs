using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIT.Infrastructure.Data.DTOs
{
    public class BaseUser
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên người dùng không được vượt quá 100 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [RegularExpression(@"^(?=.*\d)(?=.*[@$!%*?&])[A-Z][A-Za-z\d@$!%*?&]{5,}$",
        ErrorMessage = "Mật khẩu phải bắt đầu bằng chữ in hoa, chứa ít nhất 1 số và 1 ký tự đặc biệt")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Số điện thoại phải đúng 11 số")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        public string UserAddress { get; set; }
    }

    public class UserDTO : BaseUser
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string UserImage { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateUserDTO : BaseUser
    {
        public int userroleid { get; set; }
        public IFormFile image {  get; set; }
    }

    //Update User
    public class UpdateUserDTO : BaseUser
    {
        public int userroleid { get; set; }
        public IFormFile image { get; set; }
    }
}
