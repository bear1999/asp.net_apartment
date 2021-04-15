using _NSY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Commom;

namespace _NSY.Controllers
{
    public class homeController : Controller
    {
        ApartmentDataContext db = new ApartmentDataContext();
        // GET: home
        [HttpGet]
        public ActionResult Contact()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            LoadInfoBank();
            var a = (from b in db.InfoApartments
                     select b).ToList();
            foreach (var c in a)
            {
                //load lên view z cho lẹ
                ViewBag.name = c.nameApartment;
                ViewBag.addres = c.addressApartment;
                ViewBag.email = c.emailApartment;
                ViewBag.tel = c.telApartment;
                break;
            }
            return View();
        }
        [HttpGet]
        public ActionResult Error404()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            return View();
        }
        [HttpGet]
        public ActionResult doimatkhau()
        {
            if (Session["GetOTP"] == null)
                return RedirectToAction("login", "home");
            ViewData["Err"] = "(*) Mã OTP đã được gửi đến Email của bạn !";
            ViewData["Err1"] = "Lưu ý: Mã OTP chỉ có thời hạn 60 giây.";
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult doimatkhau(FormCollection collection)
        {
            if (Session["GetOTP"] == null)
                return RedirectToAction("login", "home");
            var otp = collection["GetOTP"];
            var mk1 = collection["MatKhau1"];
            var mk2 = collection["MatKhau2"];

            if (DateTime.Now > DateTime.Parse(Session["timeOut"].ToString()))
            {
                ViewData["Err"] = "(*) Đã hết thời gian timeout 60 giây xin vui lòng gửi lại OTP !";
                Session.RemoveAll();
                return View();
            }
            else if (String.IsNullOrEmpty(otp))
                ViewData["Err"] = "(*) Vui lòng nhập Mã OTP !";
            else if (String.IsNullOrEmpty(mk1))
                ViewData["Err"] = "(*) Vui lòng nhập Mật khẩu !";
            else if (String.IsNullOrEmpty(mk2))
                ViewData["Err"] = "(*) Vui lòng xác nhận lại Mật khẩu !";
            else if (mk1 != mk2)
                ViewData["Err"] = "(*) Mật khẩu nhập lại không khớp !";
            else if (!Equals(Session["GetOTP"], otp))
                ViewData["Err"] = "(*) Mã OTP không đúng !";
            else
            {
                var a = from b in db.logins
                        where Equals(b.Email, Session["GetEmail"])
                        select b;
                foreach (var c in a)
                {
                    c.Password = GetMD5(mk1);
                }
                db.SubmitChanges();
                ViewData["Err"] = "(*) Đổi mật khẩu thành công !";
                Session.RemoveAll();
            }
            return View();
        }
        [HttpGet]
        public ActionResult quenmatkhau()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult quenmatkhau(FormCollection collection)
        {
            var email = collection["Email"];
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Err"] = "(*) Vui lòng nhập Địa chỉ Email";
                return View();
            }
            else
            {
                var a = from b in db.logins
                        where Equals(b.Email, email)
                        select b;
                foreach (var c in a)
                {
                    Session["timeOut"] = DateTime.Now.AddSeconds(60);
                    Session["GetOTP"] = RandomOTP();
                    Session["GetEmail"] = email;
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/SendMail/OTP.html"));
                    content = content.Replace("{{OTP}}", Session["GetOTP"].ToString());
                    new MailHelper().SendMail(email, "[No Reply] Xác thực OTP", content);
                    return RedirectToAction("doimatkhau", "home");
                }
                ViewData["Err"] = "(*) Địa chỉ Email không tồn tại trong hệ thống !";
                return View();
            }
        }
        [HttpGet]
        public ActionResult login()
        {
            if (Session["dangnhap"] != null) return RedirectToAction("thongbaotoanha", "home");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult login(FormCollection collection, Models.login obj)
        {
            var dn = collection["Email"];
            var mk = collection["Password"];
            if (String.IsNullOrEmpty(dn))
                ViewData["Er1"] = "(*) Vui lòng nhập Email hoặc SĐT !";
            else if (String.IsNullOrEmpty(mk))
                ViewData["Er1"] = "(*) Vui lòng nhập mật khẩu !";
            else
            {
                try
                {
                    Session["dangnhap"] = null;
                    var data = from login in db.logins
                               join acc in db.accounts on login.idUser equals acc.idUser
                               join vt in db.positions on acc.idStaff equals vt.idStaff
                               where (login.Email == dn || login.PhoneNumber == dn || acc.IDCard == dn) && login.Password == GetMD5(mk)
                               select new { acc, vt.idLevel};
                    foreach (var user in data)
                    {
                        Session.RemoveAll();
                        user.acc.lastLogin = DateTime.Parse(DateTime.Now.ToString());
                        Session["dangnhap"] = user.acc.idUser;
                        Session["idChucvu"] = user.acc.idStaff;
                        Session["idLevel"] = user.idLevel;
                    }
                    db.SubmitChanges();
                    if (Session["dangnhap"] == null)
                    {
                        ViewData["Er1"] = "(*) Sai thông tin đăng nhập !";
                        return View();
                    }
                    return RedirectToAction("thongbaotoanha");
                }
                catch
                {
                    ViewData["Er1"] = "(*) Đã xảy ra lỗi vui lòng đăng nhập lại !";
                    return View();
                }
            }
            return View();
        }
        [HttpGet, ValidateInput(false)]
        public ActionResult thongbaonhanvien()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login");
            if (Int32.Parse(Session["idLevel"].ToString()) < 1) return RedirectToAction("Error404", "home");
                LoadThongBao(1); //NV Type = 1;
            return View();
        }
        [HttpGet, ValidateInput(false)]
        public ActionResult thongbaotoanha()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login");
            LoadThongBao(0); //TN Type = 0;
            return View();
        }
        public ActionResult getLogoName()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login");
            var a = from b in db.InfoApartments
                    select b;
            return PartialView(a.ToList());
        }
        public ActionResult getDangnhap()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login");
            var a = from b in db.accounts
                    where Equals(b.idUser, Session["dangnhap"])
                    select b;
            return PartialView(a.ToList());
        }
        [HttpGet]
        public ActionResult logout()
        {
            Session.RemoveAll();
            return RedirectToAction("login");
        }
        [HttpGet]
        public ActionResult thongtincanhan()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");

            DropDownListChucVu();
            var getidUser = Session["dangnhap"];
            Load2Models(getidUser.ToString());
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult thongtincanhan(FormCollection collection, HttpPostedFileBase fileAvatar)
        {
            if (Session["dangnhap"] == null)
                return RedirectToAction("login", "home");

            var getidUser = Session["dangnhap"];
            DropDownListChucVu();
            Load2Models(getidUser.ToString());

            var sdt = collection["PhoneNumber"];
            var email = collection["Email"];
            var mk = collection["Password"];
            var mk1 = collection["Password1"];
            var mkht = collection["CurrentPassword"];

            var getInfo = from acc in db.accounts
                          join logi in db.logins on acc.idUser equals logi.idUser
                          where logi.idUser != Int32.Parse(getidUser.ToString())
                          select new { acc.IDCard, logi.Email, logi.PhoneNumber };

            foreach (var info in getInfo)
            {
                if (sdt == info.PhoneNumber)
                {
                    ViewData["Err1"] = "(*) Số điện thoại này đã tồn tại trong hệ thống !";
                    return View();
                }
                else if (email == info.Email)
                {
                    ViewData["Err1"] = "(*) Địa chỉ Email này đã tồn tại trong hệ thống !";
                    return View();
                }
            }
            if (mk != mk1)
                ViewData["Err1"] = "(*) Mật khẩu nhập lại không đúng !";
            else
            {
                try
                {
                    var query1 = from acc in db.accounts
                                 join lg in db.logins on acc.idUser equals lg.idUser
                                 where Equals(lg.idUser, getidUser)
                                 select new { acc, lg };
                    foreach (var info in query1)
                    {
                        if (!Equals(info.lg.Password, GetMD5(mkht)) && !String.IsNullOrEmpty(mkht))
                        {
                            ViewData["Err1"] = "(*) Mật khẩu hiện tại không đúng !";
                            return View();
                        }
                        else if (!String.IsNullOrEmpty(mk) && !String.IsNullOrEmpty(mk1) && String.IsNullOrEmpty(mkht))
                        {
                            ViewData["Err1"] = "(*) Vui lòng nhập mật khẩu hiện tại !";
                            return View();
                        }
                        if (info.lg.PhoneNumber != null && String.IsNullOrEmpty(sdt))
                        {
                            ViewData["Err1"] = "(*) Số điện thoại không đế trống !";
                            return View();
                        }
                        else if(String.IsNullOrEmpty(sdt)) info.lg.PhoneNumber = null;
                        else info.lg.PhoneNumber = sdt;
                        if (info.lg.Email != null && String.IsNullOrEmpty(email))
                        {
                            ViewData["Err1"] = "(*) Email không đế trống !";
                            return View();
                        }
                        else if (String.IsNullOrEmpty(email)) info.lg.Email = null;
                        else info.lg.Email = email;
                        if (!String.IsNullOrEmpty(mk))
                            info.lg.Password = GetMD5(mk);
                        if (ModelState.IsValid)
                        {
                            if (fileAvatar != null)
                            {
                                string extension1 = System.IO.Path.GetExtension(fileAvatar.FileName);
                                if (Equals(extension1, ".png") || Equals(extension1, ".jpg") || Equals(extension1, ".gif"))
                                {
                                    var filename = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(fileAvatar.FileName);
                                    var path = Path.Combine(Server.MapPath("~/Assets/Image"), filename);
                                    if (System.IO.File.Exists(path))
                                        ViewData["Err1"] = "(*) Hình ảnh đã tồn tại !";
                                    else
                                    {
                                        if (info.acc.Image_Avatar != null)
                                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Image"), info.acc.Image_Avatar));
                                        fileAvatar.SaveAs(path);
                                        info.acc.Image_Avatar = filename;
                                    }
                                }
                                else
                                {
                                    ViewData["Err1"] = "(*) Chỉ được cập nhật định dạng hình ảnh (.jpg | .png | .gif)";
                                    return View();
                                }
                            }
                        }
                    }
                    db.SubmitChanges();
                    Load2Models(getidUser.ToString());
                    ViewData["Err2"] = "(*) Cập nhật thành công !";
                }
                catch
                {
                    ViewData["ErrMess1"] = "Lưu ý: ";
                    ViewData["ErrMess2"] = "- Số điện thoại không vượt quá 15 ký tự !";
                    ViewData["ErrMess3"] = "- Địa chỉ Email không vượt quá 50 ký tự !";
                    return View();
                }
            }
            return View();
        }
        [NonAction]
        private ActionResult LoadThongBao(int id)
        {
            IList<Thongbao> infoacc_one = new List<Thongbao>();
            var query = from a in db.accounts
                        join b in db.notifications_generals on a.idUser equals b.idUser
                        where Equals(b.typeUser, id)
                        orderby b.date_submited descending
                        select new { a.Username, a.Image_Avatar, b };

            var infoaccs = query.ToList();
            foreach (var info in infoaccs)
            {
                infoacc_one.Add(new Thongbao()
                {
                    Username = info.Username,
                    idNotify = info.b.idNotify,
                    notify = info.b.notify,
                    date_submited = info.b.date_submited,
                    idUser = info.b.idUser,
                    Image_Avatar = info.Image_Avatar
                });
            }
            return View(infoacc_one);
        }
        [NonAction]
        private ActionResult LoadInfoBank()
        {
            IList<InfoBank> infoacc_one = new List<InfoBank>();
            var query = from a in db.InfoBanks select a;
            var infoaccs = query.ToList();
            foreach (var info in infoaccs)
            {
                infoacc_one.Add(new InfoBank()
                {
                    nameBank = info.nameBank,
                    numberBank = info.numberBank,
                    ChiNhanhBank = info.ChiNhanhBank,
                    ownerBank = info.ownerBank
                });
            }
            return View(infoacc_one);
        }
        [NonAction]
        private ActionResult Load2Models(String getidUser)
        {
            IList<InfoAccount> infoacc_one = new List<InfoAccount>();
            var query = from acc in db.accounts
                        join lg in db.logins on acc.idUser equals lg.idUser
                        join cv in db.positions on acc.idStaff equals cv.idStaff
                        where Equals(lg.idUser, getidUser)
                        select new { acc, lg, cv };

            var infoaccs = query.ToList();
            foreach (var info in infoaccs)
            {
                infoacc_one.Add(new InfoAccount()
                {
                    idUser = info.acc.idUser,
                    Username = info.acc.Username,
                    Email = info.lg.Email,
                    PhoneNumber = info.lg.PhoneNumber,
                    name_position = info.cv.name_position,
                    IDCard = info.acc.IDCard,
                    Birthday = DateTime.Parse(info.acc.Birthday.ToString()),
                    RegDate = DateTime.Parse(info.acc.RegDate.ToString()),
                    lastLogin = DateTime.Parse(info.acc.lastLogin.ToString()),
                    Sex = info.acc.Sex,
                    Image_Avatar = info.acc.Image_Avatar,
                    idStaff = Int32.Parse(info.acc.idStaff.ToString())
                });
            }
            return View(infoacc_one);
        }
        [NonAction]
        private void DropDownListChucVu()
        {
            var dataList = new SelectList(
                            (
                                from position in db.positions
                                where position.idStaff != 0
                                select new SelectListItem { Text = position.name_position, Value = position.idStaff.ToString() }
                            ), "Value", "Text");
            ViewBag.Loadchucvu = dataList;
        }
        [NonAction]
        private static string RandomOTP()
        {
            int dodaithe = 6;
            string allowednumber = "0123456789";
            char[] chars = new char[dodaithe];
            Random rd = new Random();

            bool usenumber = true;
            for (int i = 0; i < dodaithe; i++)
            {
                if (usenumber)
                {
                    chars[i] = allowednumber[rd.Next(0, allowednumber.Length)];
                    usenumber = false;
                }
                else
                {
                    chars[i] = allowednumber[rd.Next(0, allowednumber.Length)];
                    usenumber = true;
                }
            }
            return new string(chars);
        }
        [NonAction]
        private static String GetMD5(string text)
        {
            String str = "";
            Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(text);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            foreach (Byte b in buffer)
                str += b.ToString("X2");
            return str;
        }
    }
}