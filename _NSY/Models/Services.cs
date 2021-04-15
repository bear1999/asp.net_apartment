using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace _NSY.Models
{
    public class Quanlydichvu
    {
        public IEnumerable<apartment> apartment { get; set; }
        public IEnumerable<services_type> services_type { get; set; }
        public IEnumerable<Dichvukhachhang> Dichvukhachhang { get; set; }
        public IEnumerable<Dichvukhachhang> DVGanHetHan { get; set; }
        public IEnumerable<apartment> CanHoGanHetHan { get; set; }
    }
    public class Canho
    {
        public IEnumerable<ApartmentManager> ApartmentManager { get; set; }
        public IEnumerable<InfoAccount> InfoAccount { get; set; }
        public IEnumerable<Dichvukhachhang> Dichvukhachhang { get; set; }
        public IEnumerable<log_service> LogDichvu { get; set; }
    }
    public class Thongbao
    {
        public int idNotify { get; set; }
        public string notify { get; set; }
        public DateTime date_submited { get; set; }
        public int idUser { get; set; }
        public int typeUser { get; set; }
        public string Username { get; set; }
        public string Image_Avatar { get; set; }
    }
    public class Dichvukhachhang
    {
        public int idService { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime ExpDate { get; set; }
        public int serviceType { get; set; }
        public int idHome { get; set; }
        public float value { get; set; } // hệ số sử dụng
        public string name_service { get; set; }
        public Decimal Price { get; set; }
        public bool type { get; set; } // hệ số, ko hệ số
        public string DonVi { get; set; }
        public string Reason { get; set; }
        public bool CancelOrSave { get; set; } // Lưu log kh, log hủy dịch vụ
        public int idUserCancel { get; set; }
    }
    public class Hotro
    {
        public IEnumerable<Ticket> Ticket { get; set; }
        public IEnumerable<Ticket> Quanly { get; set; }
        public IEnumerable<Ticket> Kinhdoanh { get; set; }
        public IEnumerable<Ticket> QuanlyHT { get; set; }
        public IEnumerable<Ticket> KinhdoanhHT { get; set; }
        public IEnumerable<Ticket> Noidung { get; set; }

    }
    public class Ticket
    {
        [StringLength(50, ErrorMessage = "(*) Tiêu đề không vượt quá 50 ký tự !")]
        [Required(ErrorMessage = "(*) Vui lòng nhập Tiêu đề !")]
        public string TittleTicket { get; set; }
        [Required(ErrorMessage = "(*) Vui lòng nhập Nội dụng !")]
        public string tText { get; set; }
        public DateTime DateCreate { get; set; }
        //public int idtStatus { get; set; }
        public int idTicket { get; set; }
        //public int idUser_post { get; set; }
        public DateTime DateSent { get; set; }
        public string Email { get; set; }
        public string NameStatus { get; set; }
        public string LastRep { get; set; }
        public string Avatar { get; set; }
        public bool Closed { get; set; }
    }
}