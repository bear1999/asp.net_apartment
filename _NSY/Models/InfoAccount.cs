using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _NSY.Models
{
    public class InfoAccount
    {
        //Gồm 2 table login, account
        public int idUser { get; set; }
        //[Required(ErrorMessage = "(*) Vui lòng nhập Họ tên !")]
        [StringLength(25, ErrorMessage = "(*) Họ tên không vượt quá 25 ký tự !")]
        public string Username { get; set; }
        //[Required(ErrorMessage = "(*) Vui lòng nhập Địa chỉ Email !")]
        [StringLength(50, ErrorMessage = "(*) Email không vượt quá 50 ký tự !")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "(*) Vui lòng nhập Số điện thoại !")]
        [StringLength(15, ErrorMessage = "(*) Số điện thoại không vượt quá 15 ký tự !")]
        public string PhoneNumber { get; set; }
        public string name_position { get; set; }
        public string Sex { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime lastLogin { get; set; }
        [Required(ErrorMessage = "(*) Vui lòng Nhập ngày sinh !")]
        public DateTime Birthday { get; set; }
        //[Required(ErrorMessage = "(*) Vui lòng nhập Thẻ căn cước / CMND !")]
        [StringLength(12, ErrorMessage = "(*) Thẻ căn cước / CMND không thể vượt quá 12 ký tự !")]
        public string IDCard { get; set; }
        public string Image_Avatar { get; set; }
        //[Required(ErrorMessage = "(*) Vui lòng chọn Mặt trước Thẻ căn cước / CMND !")]
        public string Image_IDCard1 { get; set; }
        //[Required(ErrorMessage = "(*) Vui lòng chọn Mặt sau Thẻ căn cước / CMND !")]
        public string Image_IDCard2 { get; set; }
        [Required(ErrorMessage = "(*) Vui lòng Nhập mã căn hộ !")]
        public int idHome { get; set; }
        public int idStaff { get; set; }
    }
}