using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace _NSY.Models
{
    public class ApartmentManager
    {
        //Gồm 3 table apartments, type_apartment, statusGeneral
        public int idMain { get; set; }  
        public string name { get; set; }
        public int statusGeneral { get; set; } //id    
        [StringLength(200, ErrorMessage = "(*) Mô tả không vượt quá 200 ký tự !")] 
        public string description { get; set; }
        public string nameStatus { get; set; }
        public int idGroup { get; set; }
        public string name_type { get; set; }
        public int typeApartment { get; set; }
        public int idSub { get; set; }
        public decimal? Rent { get; set; } //Thuê chung cư
        public DateTime? DateRent { get; set; } //Thuê chung cư
        public DateTime? ExpRent { get; set; } //Thuê chung cư
        public int DienTich { get; set; }
    }
}