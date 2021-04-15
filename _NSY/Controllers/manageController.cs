using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using _NSY.Models;
using System.Configuration;
using Commom;

namespace _NSY.Controllers
{
    public class manageController : Controller
    {
        ApartmentDataContext db = new ApartmentDataContext();
        [HttpGet]
        public ActionResult dichvuganquahan()
        {
            Quanlydichvu model = new Quanlydichvu();
            model.apartment = CanHoQuaHan();
            model.Dichvukhachhang = DichVuQuaHan();
            model.DVGanHetHan = DichVuGanHetHan();
            model.CanHoGanHetHan = CHGanHetHan();
            return View(model);
        }
        [HttpGet, ValidateInput(false)]
        public ActionResult capnhatthongbao()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Int32.Parse(Session["idLevel"].ToString()) == 0) return RedirectToAction("Error404", "home");
            var id = Url.RequestContext.RouteData.Values["id"];
            if (id != null)
            {
                var a = from b in db.notifications_generals
                        where Equals(b.idNotify, id)
                        select b;
                foreach (var c in a)
                {
                    ViewBag.Thongbao = c.notify;
                    return View();
                }
            }
            else return RedirectToAction("thongbaotoanha", "home");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult capnhatthongbao(FormCollection collection)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Int32.Parse(Session["idLevel"].ToString()) == 0) return RedirectToAction("Error404", "home");
            var id = Url.RequestContext.RouteData.Values["id"];
            if (id != null)
            {
                var capnhat = collection["CapNhat"];
                if (String.IsNullOrEmpty(capnhat) || Equals(capnhat, "<p><br></p>"))
                    ViewBag.Error = "Nội dung cập nhật không để trống";
                else
                {
                    var a = from b in db.notifications_generals
                            where Equals(b.idNotify, id)
                            select b;
                    foreach (var c in a)
                    {
                        c.notify = capnhat;
                        c.date_submited = DateTime.Parse(DateTime.Now.ToString());
                        ViewBag.Thongbao = c.notify;
                    }
                    db.SubmitChanges();
                    ViewBag.Success = "Cập nhật thành công";
                    return View();
                }
            }
            else return RedirectToAction("thongbaotoanha", "home");
            return View();
        }
        [HttpGet]
        public ActionResult xoathongbao()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Int32.Parse(Session["idLevel"].ToString()) == 0) return RedirectToAction("Error404", "home");
            var id = Url.RequestContext.RouteData.Values["id"];
            int flag = 0;
            if (id != null)
            {
                var a = from b in db.notifications_generals
                        where Equals(b.idNotify, id)
                        select b;
                foreach (var c in a)
                {
                    db.notifications_generals.DeleteOnSubmit(c);
                    flag = c.typeUser;
                }
                db.SubmitChanges();
            }
            if (flag == 0)
                return RedirectToAction("thongbaotoanha", "home");
            else return RedirectToAction("thongbaonhanvien", "home");
        }
        [HttpGet]
        public ActionResult xacnhanthanhtoancanho()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idChucvu"], 1) && !Equals(Session["idChucvu"], 3)) return RedirectToAction("Error404", "home");

            var id = Url.RequestContext.RouteData.Values["id"];
            if (id == null) return RedirectToAction("Error404", "home");
            LoadApartmentManager(id.ToString());
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult xacnhanthanhtoancanho(FormCollection collection)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idChucvu"], 1) && !Equals(Session["idChucvu"], 3)) return RedirectToAction("Error404", "home");

            var id = Url.RequestContext.RouteData.Values["id"];
            if (id == null) return RedirectToAction("Error404", "home");
            LoadApartmentManager(id.ToString());
            var thanhtoan = collection["ThanhToan"];
            if (String.IsNullOrEmpty(thanhtoan))
            {
                ViewData["Err"] = "(*) Vui lòng nhập số ngày đã thanh toán !";
                return View();
            }
            else
            {
                var money = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                var a = from b in db.apartments
                        where Equals(b.idMain, id) && Equals(b.typeApartment, 4)
                        select b;
                foreach (var c in a)
                {
                    c.DateRent = DateTime.Parse(c.ExpRent.ToString());
                    c.ExpRent = DateTime.Parse(c.ExpRent.ToString()).AddDays(Int32.Parse(thanhtoan.ToString()));

                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/SendMail/HomeConfirm.html"));
                    content = content.Replace("{{idHome}}", c.idMain.ToString());
                    content = content.Replace("{{DateConfirm}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    content = content.Replace("{{RegDate}}", c.DateRent.Value.ToString("dd/MM/yyy"));
                    content = content.Replace("{{ExpDate}}", c.ExpRent.Value.ToString("dd/MM/yyyy"));
                    content = content.Replace("{{SumPrice}}", String.Format(money, "{0:c0}", c.Rent));

                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new MailHelper().SendMail(toEmail, "[No Reply] Xác nhận gia hạn thuê căn hộ", content); //Gửi mail tới ng quản trị test

                    //Gửi đến Email khách hàng
                    var d = from g in db.logins
                            join j in db.owner_homes on g.idUser equals j.idUser
                            where Equals(j.idHome, c.idMain)
                            select g.Email;
                    foreach (var s in d)
                        if (s != null)
                            new MailHelper().SendMail(s, "[No Reply] Xác nhận gia hạn thuê căn hộ", content);

                    ViewData["success"] = "(*) Xác nhận thanh toán thành công !";
                    LoadApartmentManager(id.ToString());
                }
                db.SubmitChanges();
            }
            return View();
        }
        [HttpGet]
        public ActionResult danhsachdichvudahuy()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            DanhSachDichVuDaHuy();
            return View();
        }
        [HttpGet]
        public ActionResult xacnhanthanhtoandichvu()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idChucvu"], 1) && !Equals(Session["idChucvu"], 3)) return RedirectToAction("Error404", "home");

            var id = Url.RequestContext.RouteData.Values["id"];
            if (id == null) return RedirectToAction("Error404", "home");
            UpdateDVKhachHang(id.ToString());
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult xacnhanthanhtoandichvu(FormCollection collection, log_service log)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idChucvu"], 1) && !Equals(Session["idChucvu"], 3)) return RedirectToAction("Error404", "home");

            var id = Url.RequestContext.RouteData.Values["id"];
            if (id == null) return RedirectToAction("Error404", "home");
            UpdateDVKhachHang(id.ToString());

            var thanhtoan = collection["ThanhToan"];
            if (String.IsNullOrEmpty(thanhtoan))
            {
                ViewData["Err"] = "(*) Vui lòng nhập số ngày đã thanh toán !";
                return View();
            }
            else
            {
                var query = from a in db.services
                            join c in db.services_types on a.serviceType equals c.serviceType
                            where Equals(a.idService, id)
                            select new { a, c };
                foreach (var b in query)
                {
                    if (b.c.type == true && b.a.value == 0)
                    {
                        ViewData["Err"] = "(*) Dịch vụ HỆ SỐ này chưa cập nhật HỆ SỐ SỬ DỤNG !";
                        return View();
                    }
                    //Insert log
                    log.nameService = b.c.name_service;
                    log.RegDate = b.a.RegDate;
                    log.ExpDate = b.a.ExpDate;
                    log.HeSo = b.a.value;
                    log.Price = b.c.Price;
                    log.DonVi = b.c.DonVi;
                    log.type = b.c.type;
                    log.idHome = b.a.idHome;
                    log.CancelOrSave = true;
                    decimal tongia = 0;
                    if (b.a.value != 0)
                        tongia = decimal.Parse(b.a.value.ToString()) * b.c.Price;
                    else tongia = b.c.Price;
                    db.log_services.InsertOnSubmit(log);
                    //Đã thanh toán
                    b.a.RegDate = b.a.ExpDate;
                    b.a.ExpDate = b.a.RegDate.AddDays(Int32.Parse(thanhtoan));
                    b.a.value = 0;

                    var money = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/SendMail/ServiceConfirm.html"));
                    content = content.Replace("{{idService}}", b.a.idService.ToString());
                    content = content.Replace("{{idHome}}", b.a.idHome.ToString());
                    content = content.Replace("{{nameService}}", b.c.name_service);
                    content = content.Replace("{{DateConfirm}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    content = content.Replace("{{RegDate}}", b.a.RegDate.ToString("dd/MM/yyyy"));
                    content = content.Replace("{{ExpDate}}", b.a.ExpDate.ToString("dd/MM/yyyy"));
                    content = content.Replace("{{SumPrice}}", String.Format(money, "{0:c0}", tongia));

                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new MailHelper().SendMail(toEmail, "[No Reply] Xác nhận thanh toán dịch vụ", content); //Gửi mail tới ng quản trị test

                    //Gửi đến Email khách hàng
                    var a = from g in db.logins
                            join j in db.owner_homes on g.idUser equals j.idUser
                            where Equals(j.idHome, b.a.idHome)
                            select g.Email;
                    foreach (var s in a)
                        if (s != null)
                            new MailHelper().SendMail(s, "[No Reply] Xác nhận thanh toán dịch vụ", content);

                    ViewData["success"] = "(*) Xác nhận thanh toán thành công !";
                    UpdateDVKhachHang(id.ToString());
                }
                db.SubmitChanges();
            }
            return View();
        }
        [HttpGet]
        public ActionResult huydichvukhachhang()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idChucvu"], 1) && !Equals(Session["idChucvu"], 3)) return RedirectToAction("Error404", "home");

            var id = Url.RequestContext.RouteData.Values["id"];
            if (id != null)
            {
                UpdateDVKhachHang(id.ToString());
            }
            else return RedirectToAction("Error404", "home");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult huydichvukhachhang(FormCollection collection)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idChucvu"], 1) && !Equals(Session["idChucvu"], 3)) return RedirectToAction("Error404", "home");

            var id = Url.RequestContext.RouteData.Values["id"];
            UpdateDVKhachHang(id.ToString());
            if (id != null)
            {
                var lydo = collection["LyDo"];
                if (String.IsNullOrEmpty(lydo))
                    ViewData["Err"] = "(*) Vui lòng nhập lý do cần hủy";
                else
                {
                    try
                    {
                        var query = from a in db.services
                                    join c in db.services_types on a.serviceType equals c.serviceType
                                    where Equals(a.idService, id)
                                    select new { a, c };
                        foreach (var b in query)
                        {
                            //Inset log
                            log_service log = new log_service();
                            log.nameService = b.c.name_service;
                            log.RegDate = b.a.RegDate;
                            log.ExpDate = b.a.ExpDate;
                            log.HeSo = b.a.value;
                            log.Price = b.c.Price;
                            log.DonVi = b.c.DonVi;
                            log.type = b.c.type;
                            log.idHome = b.a.idHome;
                            log.CancelOrSave = false;
                            decimal tongia = 0;
                            if (b.a.value != 0)
                                tongia = decimal.Parse(b.a.value.ToString()) * b.c.Price;
                            else tongia = b.c.Price;
                            log.idUserCancel = Int32.Parse(Session["dangnhap"].ToString());
                            db.log_services.InsertOnSubmit(log);
                            db.SubmitChanges();

                            log_reasonCancel reason = new log_reasonCancel();
                            reason.idLog_Service = log.idLogService;
                            reason.Reason = lydo;
                            db.log_reasonCancels.InsertOnSubmit(reason);
                            db.SubmitChanges();

                            db.services.DeleteOnSubmit(b.a);
                            db.SubmitChanges();
                        }

                        return RedirectToAction("quanlydichvu", "manage");
                    }
                    catch
                    {
                        ViewData["Err"] = "(*) Lý do hủy không vượt quá 40 ký tự !";
                        return View();
                    }
                }
            }
            else return RedirectToAction("Error404", "home");
            return View();
        }
        [HttpGet]
        public ActionResult quanlythongbao()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult quanlythongbao(FormCollection collection, notifications_general thongbao)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            var dangthongbao = collection["DangThongBao"];
            var type = collection["typeUser"];
            if (String.IsNullOrEmpty(dangthongbao) || Equals(dangthongbao, "<p><br></p>"))
                ViewBag.Error = "Vui lòng soạn thông báo";
            else
            {
                thongbao.notify = dangthongbao;
                thongbao.date_submited = DateTime.Parse(DateTime.Now.ToString());
                thongbao.idUser = Int32.Parse(Session["dangnhap"].ToString());
                thongbao.typeUser = Int32.Parse(type.ToString());
                db.notifications_generals.InsertOnSubmit(thongbao);
                db.SubmitChanges();
                ViewBag.Success = "Đăng thông báo thành công";
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult themkhachhang()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Int32.Parse(Session["idChucvu"].ToString()) < 2) return RedirectToAction("Error404", "home");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult themkhachhang(FormCollection collection, account dangky, login dangky1, HttpPostedFileBase fileAvatar, HttpPostedFileBase fileIDCard1, HttpPostedFileBase fileIDCard2)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Int32.Parse(Session["idChucvu"].ToString()) < 2) return RedirectToAction("Error404", "home");

            var tendn = collection["Username"];
            var ngaysinh = String.Format("{0:MM/dd/yyy}", collection["Birthday"]);
            var gt = collection["Sex"];
            var sdt = collection["PhoneNumber"];
            var cmnd = collection["IDCard"];
            var email = collection["Email"];
            var mk = collection["Password"];
            var mk1 = collection["Password1"];
            var idhome = collection["idHome"];

            var getInfo = from acc in db.accounts
                          join logi in db.logins on acc.idUser equals logi.idUser
                          select new { acc.IDCard, logi.Email, logi.PhoneNumber };

            foreach (var info in getInfo)
            {
                if (cmnd == info.IDCard)
                {
                    ViewData["Err1"] = "(*) Thẻ căn cước / CMND này đã tồn tại trong hệ thống !";
                    return View();
                }
                else if (sdt == info.PhoneNumber)
                {
                    ViewData["Err1"] = "(*) Số điện thoại này đã tồn tại trong hệ thống !";
                    return View();
                }
                else if (email == info.Email)
                {
                    ViewData["Err1"] = "(*) Địa chỉ Email này đã tồn tại trong hế thống !";
                    return View();
                }
            }

            if (String.IsNullOrEmpty(tendn))
                ViewData["Err1"] = "(*) Họ tên không để trống !";
            else if (String.IsNullOrEmpty(ngaysinh))
                ViewData["Err1"] = "(*) Ngày sinh không để trống !";
            else if (String.IsNullOrEmpty(gt))
                ViewData["Err1"] = "(*) Vui lòng chọn Giới tính !";
            else if (String.IsNullOrEmpty(idhome))
                ViewData["Err1"] = "(*) Mã căn hộ không để trống !";
            else if (mk != mk1)
                ViewData["Err1"] = "(*) Mật khẩu nhập lại không đúng !";
            else
            {
                int flagHome = 0;
                var a = from b in db.apartments
                        join c in db.statusGenerals on b.statusGeneral equals c.idStatus
                        where Equals(b.idMain, idhome) && Equals(b.typeApartment, 4) //id home là 4
                        select new { b, c };
                foreach (var d in a)
                {
                    if (Equals(d.c.idGroup, 0))
                        flagHome = 1;
                    else flagHome = 2; // Thì thôi, không có gì xảy ra
                }
                if (flagHome == 0)
                {
                    ViewData["Err1"] = "(*) Mã căn hộ không tồn tại !";
                    return View();
                }
                else if (flagHome == 1)
                {
                    ViewData["Err1"] = "(*) Căn hộ này đang ở trạng thái KHÔNG THỂ SỬ DỤNG vui lòng cập nhật lại trạng thái !";
                    return View();
                }
                dangky.Username = tendn;
                dangky.Birthday = DateTime.Parse(ngaysinh);
                dangky.Sex = gt;
                if (!String.IsNullOrEmpty(cmnd)) dangky.IDCard = cmnd;
                else dangky.IDCard = null;
                dangky.idStaff = 0;
                dangky.RegDate = DateTime.Parse(DateTime.Now.ToString());
                dangky.lastLogin = DateTime.Parse(DateTime.Now.ToString());
                dangky.Image_Avatar = null;

                if (ModelState.IsValid)
                {
                    if (fileAvatar != null)
                    {
                        string extension1 = System.IO.Path.GetExtension(fileAvatar.FileName);
                        if (!Equals(extension1, ".png") || !Equals(extension1, ".jpg") || !Equals(extension1, ".gif"))
                        {
                            ViewData["Err1"] = "(*) Chỉ được cập nhật định dạng hình ảnh (.jpg | .png | .gif)";
                            return View();
                        }
                    }
                    string extension2 = System.IO.Path.GetExtension(fileIDCard1.FileName);
                    string extension3 = System.IO.Path.GetExtension(fileIDCard2.FileName);
                    if ((Equals(extension2, ".png") || Equals(extension2, ".jpg") || Equals(extension2, ".gif")) &&
                        (Equals(extension3, ".png") || Equals(extension3, ".jpg") || Equals(extension3, ".gif")))
                    {
                        var filename1 = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(fileIDCard1.FileName);
                        var path1 = Path.Combine(Server.MapPath("~/Assets/Image"), filename1);
                        var filename2 = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(fileIDCard2.FileName);
                        var path2 = Path.Combine(Server.MapPath("~/Assets/Image"), filename2);

                        if (System.IO.File.Exists(path1) || System.IO.File.Exists(path2))
                            ViewData["Err1"] = "(*) Hình ảnh đã tồn tại !";
                        else
                        {
                            if (fileAvatar != null)
                            {
                                var filename = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(fileAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/Assets/Image"), filename);
                                if (System.IO.File.Exists(path))
                                    ViewData["Err1"] = "(*) Hình ảnh đã tồn tại !";
                                else
                                {
                                    fileAvatar.SaveAs(path);
                                    dangky.Image_Avatar = filename;
                                }
                            }
                            fileIDCard1.SaveAs(path1);
                            fileIDCard2.SaveAs(path2);
                            dangky.Image_IDCard1 = filename1;
                            dangky.Image_IDCard2 = filename2;
                        }
                    }
                    else
                    {
                        ViewData["Err1"] = "(*) Chỉ được cập nhật định dạng hình ảnh (.jpg | .png | .gif)";
                        return View();
                    }
                }
                db.accounts.InsertOnSubmit(dangky);
                db.SubmitChanges();

                //----
                owner_home owner = new owner_home();
                owner.idHome = Int32.Parse(idhome);
                owner.idUser = dangky.idUser;
                db.owner_homes.InsertOnSubmit(owner);
                db.SubmitChanges();
                //----

                dangky1.idUser = dangky.idUser;
                if (!String.IsNullOrEmpty(sdt)) dangky1.PhoneNumber = sdt;
                else dangky1.PhoneNumber = null;
                if (!String.IsNullOrEmpty(email)) dangky1.Email = email;
                else dangky1.Email = null;
                if (!String.IsNullOrEmpty(mk)) dangky1.Password = GetMD5(mk);
                else dangky1.Password = null;
                db.logins.InsertOnSubmit(dangky1);
                db.SubmitChanges();
                ViewData["Reg1"] = "Tên khách hàng: " + tendn;
                ViewData["Reg2"] = "Mã căn hộ: " + idhome;

                if (!String.IsNullOrEmpty(email))
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/SendMail/RegCustomer.html"));
                    content = content.Replace("{{idHome}}", idhome);
                    content = content.Replace("{{Username}}", tendn);
                    content = content.Replace("{{PhoneNumber}}", sdt);
                    content = content.Replace("{{Email}}", email);
                    content = content.Replace("{{Password}}", mk);

                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new MailHelper().SendMail(toEmail, "[No Reply] Thông tin khách hàng", content); //Gửi mail tới ng quản trị test

                    //Gửi đến Email khách hàng
                    var xz = from g in db.logins
                             where Equals(g.Email, email)
                             select g.Email;
                    foreach (var s in xz)
                        if (s != null)
                            new MailHelper().SendMail(s, "[No Reply] Thông tin khách hàng", content);
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult dangkykhachhang()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Int32.Parse(Session["idChucvu"].ToString()) < 2) return RedirectToAction("Error404", "home");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult dangkykhachhang(FormCollection collection, account dangky, login dangky1, HttpPostedFileBase fileAvatar, HttpPostedFileBase fileIDCard1, HttpPostedFileBase fileIDCard2)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Int32.Parse(Session["idChucvu"].ToString()) < 2) return RedirectToAction("Error404", "home");

            var tendn = collection["Username"];
            var ngaysinh = String.Format("{0:MM/dd/yyy}", collection["Birthday"]);
            var gt = collection["Sex"];
            var sdt = collection["PhoneNumber"];
            var cmnd = collection["IDCard"];
            var email = collection["Email"];
            var mk = collection["Password"];
            var mk1 = collection["Password1"];
            var idhome = collection["idHome"];
            var getInfo = from acc in db.accounts
                          join logi in db.logins on acc.idUser equals logi.idUser
                          select new { acc.IDCard, logi.Email, logi.PhoneNumber };

            foreach (var info in getInfo)
            {
                if (cmnd == info.IDCard)
                {
                    ViewData["Err1"] = "(*) Thẻ căn cước / CMND này đã tồn tại trong hệ thống !";
                    return View();
                }
                else if (sdt == info.PhoneNumber)
                {
                    ViewData["Err1"] = "(*) Số điện thoại này đã tồn tại trong hệ thống !";
                    return View();
                }
                else if (email == info.Email)
                {
                    ViewData["Err1"] = "(*) Địa chỉ Email này đã tồn tại trong hế thống !";
                    return View();
                }
            }

            if (String.IsNullOrEmpty(tendn))
                ViewData["Err1"] = "(*) Họ tên không để trống !";
            else if (String.IsNullOrEmpty(ngaysinh))
                ViewData["Err1"] = "(*) Ngày sinh không để trống !";
            else if (String.IsNullOrEmpty(gt))
                ViewData["Err1"] = "(*) Vui lòng chọn Giới tính !";
            else if (String.IsNullOrEmpty(sdt))
                ViewData["Err1"] = "(*) Số điện thoại không để trống !";
            else if (String.IsNullOrEmpty(cmnd))
                ViewData["Err1"] = "(*) Thẻ căn cước / CMND không để trống !";
            else if (String.IsNullOrEmpty(email))
                ViewData["Err1"] = "(*) Địa chỉ Email không để trống !";
            else if (String.IsNullOrEmpty(idhome))
                ViewData["Err1"] = "(*) Mã căn hộ không để trống !";
            else if (String.IsNullOrEmpty(mk))
                ViewData["Err1"] = "(*) Mật khẩu không để trống !";
            else if (String.IsNullOrEmpty(mk1))
                ViewData["Err1"] = "(*) Nhập lại mật khẩu không để trống !";
            else if (fileIDCard1 == null)
                ViewData["Err1"] = "(*) Ảnh mặt trước Thẻ căn cước / CMD không để trống !";
            else if (fileIDCard2 == null)
                ViewData["Err1"] = "(*) Ảnh sau trước Thẻ căn cước / CMD không để trống !";
            else if (mk != mk1)
                ViewData["Err1"] = "(*) Mật khẩu nhập lại không đúng !";
            else
            {
                int flagHome = 0;
                var a = from b in db.apartments
                        join c in db.statusGenerals on b.statusGeneral equals c.idStatus
                        where Equals(b.idMain, idhome) && Equals(b.typeApartment, 4) //id home là 4
                        select new { b, c };
                foreach (var d in a)
                {
                    if (Equals(d.c.idGroup, 0))
                        flagHome = 1;
                    else flagHome = 2; // Thì thôi, không có gì xảy ra
                }
                if (flagHome == 0)
                {
                    ViewData["Err1"] = "(*) Mã căn hộ không tồn tại !";
                    return View();
                }
                else if (flagHome == 1)
                {
                    ViewData["Err1"] = "(*) Căn hộ này đang ở trạng thái KHÔNG THỂ SỬ DỤNG vui lòng cập nhật lại trạng thái !";
                    return View();
                }
                dangky.Username = tendn;
                dangky.Birthday = DateTime.Parse(ngaysinh);
                dangky.Sex = gt;
                dangky.IDCard = cmnd;
                dangky.idStaff = 0;
                dangky.RegDate = DateTime.Parse(DateTime.Now.ToString());
                dangky.lastLogin = DateTime.Parse(DateTime.Now.ToString());
                dangky.Image_Avatar = null;

                if (ModelState.IsValid)
                {
                    if (fileAvatar != null)
                    {
                        string extension1 = System.IO.Path.GetExtension(fileAvatar.FileName);
                        if (!Equals(extension1, ".png") || !Equals(extension1, ".jpg") || !Equals(extension1, ".gif"))
                        {
                            ViewData["Err1"] = "(*) Chỉ được cập nhật định dạng hình ảnh (.jpg | .png | .gif)";
                            return View();
                        }
                    }
                    string extension2 = System.IO.Path.GetExtension(fileIDCard1.FileName);
                    string extension3 = System.IO.Path.GetExtension(fileIDCard2.FileName);
                    if ((Equals(extension2, ".png") || Equals(extension2, ".jpg") || Equals(extension2, ".gif")) &&
                        (Equals(extension3, ".png") || Equals(extension3, ".jpg") || Equals(extension3, ".gif")))
                    {
                        var filename1 = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(fileIDCard1.FileName);
                        var path1 = Path.Combine(Server.MapPath("~/Assets/Image"), filename1);
                        var filename2 = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(fileIDCard2.FileName);
                        var path2 = Path.Combine(Server.MapPath("~/Assets/Image"), filename2);

                        if (System.IO.File.Exists(path1) || System.IO.File.Exists(path2))
                            ViewData["Err1"] = "(*) Hình ảnh đã tồn tại !";
                        else
                        {
                            if (fileAvatar != null)
                            {
                                var filename = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(fileAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/Assets/Image"), filename);
                                if (System.IO.File.Exists(path))
                                    ViewData["Err1"] = "(*) Hình ảnh đã tồn tại !";
                                else
                                {
                                    fileAvatar.SaveAs(path);
                                    dangky.Image_Avatar = filename;
                                }
                            }
                            fileIDCard1.SaveAs(path1);
                            fileIDCard2.SaveAs(path2);
                            dangky.Image_IDCard1 = filename1;
                            dangky.Image_IDCard2 = filename2;
                        }
                    }
                    else
                    {
                        ViewData["Err1"] = "(*) Chỉ được cập nhật định dạng hình ảnh (.jpg | .png | .gif)";
                        return View();
                    }
                }
                db.accounts.InsertOnSubmit(dangky);
                db.SubmitChanges();

                //----
                owner_home owner = new owner_home();
                owner.idHome = Int32.Parse(idhome);
                owner.idUser = dangky.idUser;
                db.owner_homes.InsertOnSubmit(owner);
                db.SubmitChanges();
                //----

                dangky1.PhoneNumber = sdt;
                dangky1.Email = email;
                dangky1.Password = GetMD5(mk);
                dangky1.idUser = dangky.idUser;
                db.logins.InsertOnSubmit(dangky1);
                db.SubmitChanges();
                ViewData["Reg1"] = "Tên khách hàng: " + tendn;
                ViewData["Reg2"] = "Mã căn hộ: " + idhome;

                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/SendMail/RegCustomer.html"));
                content = content.Replace("{{idHome}}", idhome);
                content = content.Replace("{{Username}}", tendn);
                content = content.Replace("{{PhoneNumber}}", sdt);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Password}}", mk);

                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendMail(toEmail, "[No Reply] Thông tin khách hàng", content); //Gửi mail tới ng quản trị test

                //Gửi đến Email khách hàng
                var xz = from g in db.logins
                         where Equals(g.Email, email)
                         select g.Email;
                foreach (var s in xz)
                    if (s != null)
                        new MailHelper().SendMail(s, "[No Reply] Thông tin khách hàng", content);
            }
            return View();
        }
        [HttpGet]
        public ActionResult quanlykhachhang()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");

            IList<InfoAccount> infoacc = new List<InfoAccount>();
            var query = from acc in db.accounts
                        join lg in db.logins on acc.idUser equals lg.idUser
                        where acc.idStaff == 0
                        orderby acc.RegDate descending
                        select new { acc, lg };

            var infoaccs = query.ToList();
            foreach (var info in infoaccs)
            {
                infoacc.Add(new InfoAccount()
                {
                    idUser = info.acc.idUser,
                    Username = info.acc.Username,
                    Sex = info.acc.Sex,
                    Email = info.lg.Email,
                    PhoneNumber = info.lg.PhoneNumber,
                });
            }
            return View(infoacc);
        }
        [HttpGet]
        public ActionResult chitietcanho()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            var getidUser = Url.RequestContext.RouteData.Values["id"];
            if (getidUser == null) return RedirectToAction("Error404", "home");
            else if (getidUser != null && Equals(Session["idLevel"], 0) && (Int32.Parse(getidUser.ToString()) != Int32.Parse(Session["dangnhap"].ToString()))) return RedirectToAction("Error404", "home");
            LoadChitietcanho(getidUser.ToString());
            return View();
        }
        public ActionResult capnhatkhachhang()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Int32.Parse(Session["idLevel"].ToString()) == 0) return RedirectToAction("Error404", "home");

            var getidUser = Url.RequestContext.RouteData.Values["id"];
            Load2Models(getidUser.ToString());
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult capnhatkhachhang(FormCollection collection, HttpPostedFileBase fileAvatar, HttpPostedFileBase fileIDCard1, HttpPostedFileBase fileIDCard2, string updateAccount)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Int32.Parse(Session["idChucvu"].ToString()) < 2) return RedirectToAction("Error404", "home");

            var getidUser = Url.RequestContext.RouteData.Values["id"];
            Load2Models(getidUser.ToString());

            if (!String.IsNullOrEmpty(updateAccount))
            {
                var tendn = collection["Username"];
                var ngaysinh = String.Format("{0:MM/dd/yyy}", collection["Birthday"]);
                var gt = collection["Sex"];
                var sdt = collection["PhoneNumber"];
                var cmnd = collection["IDCard"];
                var email = collection["Email"];
                var mk = collection["Password"];
                var mk1 = collection["Password1"];
                var idhome = collection["AddidHome"];
                var getInfo = from acc in db.accounts
                              join logi in db.logins on acc.idUser equals logi.idUser
                              where logi.idUser != Int32.Parse(getidUser.ToString())
                              select new { acc.IDCard, logi.Email, logi.PhoneNumber };

                foreach (var info in getInfo)
                {
                    if (cmnd == info.IDCard)
                    {
                        ViewData["Err1"] = "(*) Thẻ căn cước / CMND này đã tồn tại trong hệ thống !";
                        return View();
                    }
                    else if (sdt == info.PhoneNumber)
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
                if (String.IsNullOrEmpty(tendn))
                    ViewData["Err1"] = "(*) Họ tên không để trống !";
                else if (String.IsNullOrEmpty(ngaysinh))
                    ViewData["Err1"] = "(*) Ngày sinh không để trống !";
                else if (String.IsNullOrEmpty(gt))
                    ViewData["Err1"] = "(*) Vui lòng chọn Giới tính !";
                //else if (String.IsNullOrEmpty(sdt))
                //    ViewData["Err1"] = "(*) Số điện thoại không để trống !";
                //else if (String.IsNullOrEmpty(cmnd))
                //    ViewData["Err1"] = "(*) Thẻ căn cước / CMND không để trống !";
                //else if (String.IsNullOrEmpty(email))
                //    ViewData["Err1"] = "(*) Địa chỉ Email không để trống !";
                else if (mk != mk1)
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
                            if (!String.IsNullOrEmpty(idhome))
                            {
                                int flagHome = 0;
                                var a = from b in db.apartments
                                        join c in db.statusGenerals on b.statusGeneral equals c.idStatus
                                        where Equals(b.idMain, idhome) && Equals(b.typeApartment, 4) //id home là 4
                                        select new { b, c };
                                foreach (var d in a)
                                {
                                    if (Equals(d.c.idGroup, 0))
                                        flagHome = 1;
                                    else flagHome = 2; // Thì thôi, không có gì xảy ra
                                }
                                if (flagHome == 0)
                                {
                                    ViewData["Err1"] = "(*) Mã căn hộ không tồn tại !";
                                    return View();
                                }
                                else if (flagHome == 1)
                                {
                                    ViewData["Err1"] = "(*) Căn hộ này đang ở trạng thái KHÔNG THỂ SỬ DỤNG vui lòng cập nhật lại trạng thái !";
                                    return View();
                                }
                                var az = from ab in db.owner_homes
                                         join ac in db.logins on ab.idUser equals ac.idUser
                                         where Equals(ab.idHome, idhome) && Equals(ab.idUser, getidUser)
                                         select ab;
                                foreach (var an in az)
                                {
                                    ViewData["Err1"] = "(*) Khách hàng đã ở trong căn hộ này rồi !";
                                    return View();
                                }
                            }
                            info.acc.Username = tendn;
                            info.acc.Birthday = DateTime.Parse(ngaysinh);
                            info.acc.Sex = gt;
                            //CMND
                            if (!String.IsNullOrEmpty(cmnd)) info.acc.IDCard = cmnd;
                            else info.acc.IDCard = null;
                            //SDT
                            if (!String.IsNullOrEmpty(sdt)) info.lg.PhoneNumber = sdt;
                            else info.lg.PhoneNumber = null;
                            //Email
                            if (!String.IsNullOrEmpty(email)) info.lg.Email = email;
                            else info.lg.Email = null;
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
                                if (fileIDCard1 != null)
                                {
                                    string extension1 = System.IO.Path.GetExtension(fileIDCard1.FileName);
                                    if (Equals(extension1, ".png") || Equals(extension1, ".jpg") || Equals(extension1, ".gif"))
                                    {
                                        var filename1 = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(fileIDCard1.FileName);
                                        var path1 = Path.Combine(Server.MapPath("~/Assets/Image"), filename1);
                                        if (System.IO.File.Exists(path1))
                                            ViewData["Err1"] = "(*) Hình ảnh đã tồn tại !";
                                        else
                                        {
                                            if (info.acc.Image_IDCard1 != null)
                                                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Image"), info.acc.Image_IDCard1));
                                            fileIDCard1.SaveAs(path1);
                                            info.acc.Image_IDCard1 = filename1;
                                        }
                                    }
                                    else
                                    {
                                        ViewData["Err1"] = "(*) Chỉ được cập nhật định dạng hình ảnh (.jpg | .png | .gif)";
                                        return View();
                                    }
                                }
                                if (fileIDCard2 != null)
                                {
                                    string extension1 = System.IO.Path.GetExtension(fileIDCard2.FileName);
                                    if (Equals(extension1, ".png") || Equals(extension1, ".jpg") || Equals(extension1, ".gif"))
                                    {
                                        var filename2 = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(fileIDCard2.FileName);
                                        var path2 = Path.Combine(Server.MapPath("~/Assets/Image"), filename2);
                                        if (System.IO.File.Exists(path2))
                                            ViewData["Err1"] = "(*) Hình ảnh đã tồn tại !";
                                        else
                                        {
                                            if (info.acc.Image_IDCard2 != null)
                                                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Image"), info.acc.Image_IDCard2));
                                            fileIDCard2.SaveAs(path2);
                                            info.acc.Image_IDCard2 = filename2;
                                        }
                                    }
                                    else
                                    {
                                        ViewData["Err1"] = "(*) Chỉ được cập nhật định dạng hình ảnh (.jpg | .png | .gif)";
                                        return View();
                                    }
                                }
                            }
                            if (!String.IsNullOrEmpty(idhome))
                            {
                                owner_home own = new owner_home();
                                own.idUser = info.acc.idUser;
                                own.idHome = Int32.Parse(idhome);
                                db.owner_homes.InsertOnSubmit(own);
                                db.SubmitChanges();
                            }
                        }
                        db.SubmitChanges();
                        Load2Models(getidUser.ToString());
                        ViewData["Err2"] = "(*) Cập nhật thành công !";
                    }
                    catch
                    {
                        ViewData["ErrMess1"] = "Lưu ý:";
                        ViewData["ErrMess2"] = "- Họ tên không vượt quá 25 ký tự";
                        ViewData["ErrMess3"] = "- Số điện thoại không vượt quá 15 ký tự";
                        ViewData["ErrMess4"] = "- Thẻ căn cước / CMND không vượt quá 12 ký tự";
                        ViewData["ErrMess5"] = "-Địa chỉ Email không vượt quá 50 ký tự";
                        return View();
                    }
                }
            }
            else
            {
                if (Equals(Session["dangnhap"], getidUser))
                {
                    ViewData["Err1"] = "(*) Bạn không thể tự xóa tài khoản của mình !";
                    return View();
                }
                else
                {
                    var deleteAcc = from lg in db.logins
                                    join acc in db.accounts on lg.idUser equals acc.idUser
                                    join own in db.owner_homes on acc.idUser equals own.idUser
                                    where Equals(acc.idUser, getidUser)
                                    select new { lg, acc, own };
                    foreach (var del in deleteAcc)
                    {
                        db.logins.DeleteOnSubmit(del.lg);
                        db.owner_homes.DeleteOnSubmit(del.own);
                        db.accounts.DeleteOnSubmit(del.acc);
                        if (del.acc.Image_Avatar != null)
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Image"), del.acc.Image_Avatar));
                        if (del.acc.Image_IDCard1 != null)
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Image"), del.acc.Image_IDCard1));
                        if (del.acc.Image_IDCard2 != null)
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Image"), del.acc.Image_IDCard2));
                    }
                    db.SubmitChanges();
                    return RedirectToAction("quanlykhachhang");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult danhsach()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");

            var getType = Url.RequestContext.RouteData.Values["id"];
            if (getType == null) return RedirectToAction("quanlychungcu");
            LoadApartmentManager(getType.ToString());
            return View();
        }
        public ActionResult chungcu()
        {
            if (Session["dangnhap"] == null)
                return RedirectToAction("login", "home");
            var getidMain = Url.RequestContext.RouteData.Values["id"];
            if (getidMain == null) return RedirectToAction("toanha");
            var getinfo = from block in db.apartments
                          where Equals(block.idMain, getidMain)
                          select block;
            foreach (var get in getinfo)
            {
                TempData["idMain"] = get.idMain;
                LoadApartmentManagers(get.idMain, get.typeApartment + 1);
            }
            return View();
        }
        [HttpGet]
        public ActionResult home()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            return chungcu();
        }
        [HttpGet]
        public ActionResult floor()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            return chungcu();
        }
        [HttpGet]
        public ActionResult block()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            return chungcu();
        }
        [HttpGet]
        public ActionResult toanha()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");

            LoadApartmentManager("1"); //1 là Type Chung cư, vào thì load chung cư đầu tiên   
            int countChungcu = 0, countBlock = 0, countFloor = 0, countHome = 0;
            var countApartment = from chungcu in db.apartments
                                 select chungcu.typeApartment;
            foreach (var dem in countApartment)
            {
                if (dem == 1) //Chung cư
                    countChungcu++;
                else if (dem == 2) //Block
                    countBlock++;
                else if (dem == 3) //Tầng
                    countFloor++;
                else if (dem == 4) //Home
                    countHome++;
            }
            ViewData["countChungcu"] = countChungcu;
            ViewData["countBlock"] = countBlock;
            ViewData["countFloor"] = countFloor;
            ViewData["countHome"] = countHome;
            return View();
        }
        public ActionResult xoachungcu()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");
            var getidMain = Url.RequestContext.RouteData.Values["id"];
            if (getidMain == null) return RedirectToAction("Error404", "home");

            var a = from log in db.log_services
                    where Equals(log.idHome, getidMain)
                    select log;
            foreach (var b in a)
                db.log_services.DeleteOnSubmit(b);
            var c = from dv in db.services
                    where Equals(dv.idHome, getidMain)
                    select dv;
            foreach (var d in c)
                db.services.DeleteOnSubmit(d);
            var e = from h in db.apartments
                    where Equals(h.idMain, getidMain)
                    select h;
            foreach (var g in e)
                db.apartments.DeleteOnSubmit(g);

            db.SubmitChanges();
            return RedirectToAction("toanha", "manage");
        }
        [HttpGet]
        public ActionResult capnhatchungcu()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            var getid = Url.RequestContext.RouteData.Values["id"];
            if (getid == null)
                return RedirectToAction("toanha");
            else if (Int32.Parse(getid.ToString()) == 0)
                ViewData["getTypeName"] = "Chung cư";
            else
            {
                var get = from cc in db.apartments
                          join type in db.apartments_types on cc.typeApartment equals type.idType
                          where Equals(cc.idMain, getid)
                          select type;
                foreach (var type in get)
                {
                    ViewData["getTypeName"] = type.name_type;
                }
            }
            LoadApartmentManager(getid.ToString());
            DropDownListStatusGeneral();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult capnhatchungcu(FormCollection collection)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Int32.Parse(Session["idChucvu"].ToString()) < 2) return RedirectToAction("Error404", "home");

            var getidMain = Url.RequestContext.RouteData.Values["id"];
            LoadApartmentManager(getidMain.ToString());
            DropDownListStatusGeneral();
            var ten = collection["name"];
            var mota = collection["description"];
            var trangthai = collection["idStatus"];
            var rent = collection["rent"];
            var daterent = collection["daterent"];
            var exprent = collection["exprent"];
            var date = collection["date"];
            var dt = collection["DienTich"];
            int saveType = 0;
            var getType = from cc in db.apartments
                          join type in db.apartments_types on cc.typeApartment equals type.idType
                          where Equals(cc.idMain, getidMain)
                          select type;
            foreach (var get in getType)
            {
                ViewData["getTypeName"] = get.name_type;
                saveType = get.idType;
            }

            if (String.IsNullOrEmpty(ten))
                ViewData["Err1"] = "(*) Tên không để trống !";
            else
            {
                ViewData["Reg1"] = "(*) Cập nhật thành công !";
                var get = from apartment in db.apartments
                          where Equals(apartment.idMain, getidMain)
                          select apartment;
                foreach (var update in get)
                {

                    update.name = ten;
                    if (saveType == 4)
                        update.DienTich = Int32.Parse(dt);
                    if (!String.IsNullOrEmpty(trangthai))
                        update.statusGeneral = Int32.Parse(trangthai);

                    if (String.IsNullOrEmpty(daterent))
                        update.DateRent = null;
                    else update.DateRent = DateTime.Parse(daterent);
                    if (String.IsNullOrEmpty(exprent))
                        update.ExpRent = null;
                    else update.ExpRent = DateTime.Parse(exprent);
                    if (!String.IsNullOrEmpty(date))
                        update.ExpRent = DateTime.Parse(update.ExpRent.ToString()).AddDays(Double.Parse(date));
                    if (String.IsNullOrEmpty(rent))
                        update.Rent = null;
                    else update.Rent = Decimal.Parse(rent);
                    if (String.IsNullOrEmpty(mota))
                        update.description = null;
                    else update.description = mota;

                    if (!String.IsNullOrEmpty(trangthai))
                    {
                        int flagUpdate = 0;
                        //Cập nhật dịch vụ idGroup = 0 && có người ở thì thôi, ko có người ở thì xóa dịch vụ
                        var checkinHome = from inHome in db.owner_homes
                                          where Equals(inHome.idHome, getidMain)
                                          select inHome;
                        foreach (var check in checkinHome)
                        {
                            update.statusGeneral = Int32.Parse(trangthai);
                            flagUpdate = 1;
                            break;
                        }

                        if (flagUpdate == 0)
                        {
                            var a = from b in db.statusGenerals
                                    join c in db.apartments on b.idStatus equals c.statusGeneral
                                    where Equals(c.typeApartment, 4) && Equals(c.idMain, getidMain)
                                    select b;
                            foreach (var d in a)
                            {
                                var group = from aa in db.statusGenerals
                                            where Equals(aa.idStatus, trangthai)
                                            select aa;
                                foreach (var ab in group)
                                {
                                    if (Equals(ab.idGroup, 0)) // idGroup = 0, false ko xài đc
                                    {
                                        update.statusGeneral = Int32.Parse(trangthai);
                                        var e = from f in db.services
                                                where Equals(f.idHome, getidMain)
                                                select f;
                                        foreach (var g in e)
                                            db.services.DeleteOnSubmit(g);

                                        var j = from c in db.log_services
                                                where Equals(c.idHome, getidMain) && c.CancelOrSave == true
                                                select c;
                                        foreach (var k in j)
                                            db.log_services.DeleteOnSubmit(k);
                                    }
                                    else // idGroup = 1, true xài đc
                                    {
                                        update.statusGeneral = Int32.Parse(trangthai);

                                        //Đăng ký dịch vụ điện, nước tự động
                                        var az = from b in db.statusGenerals
                                                 join c in db.apartments on b.idStatus equals c.statusGeneral
                                                 where Equals(c.typeApartment, 4)
                                                 select b;
                                        foreach (var da in az)
                                        {
                                            bool flagCapnhat = false;
                                            var ba = from bb in db.services
                                                     join bc in db.services_types on bb.serviceType equals bc.serviceType
                                                     where Equals(bb.serviceType, bc.serviceType) && Equals(bb.idHome, getidMain)
                                                     select new { bc, bb };
                                            foreach (var ax in ba)
                                                flagCapnhat = true;

                                            if (Equals(da.idGroup, 1) && flagCapnhat == false)
                                            {
                                                for (int i = 1; i < 3; i++)
                                                {
                                                    service e = new service();
                                                    e.RegDate = DateTime.Parse(DateTime.Now.ToString());
                                                    e.ExpDate = DateTime.Parse(DateTime.Now.ToString()).AddDays(30);
                                                    e.serviceType = i; // Dịch vụ điện, nước
                                                    e.idHome = Int32.Parse(getidMain.ToString());
                                                    e.value = 0;
                                                    db.services.InsertOnSubmit(e);
                                                    db.SubmitChanges();
                                                }
                                                return LoadApartmentManager(getidMain.ToString());
                                            }
                                        }
                                        break;
                                    }
                                    break;
                                }
                                break;
                            }
                        }
                    }
                }
                db.SubmitChanges();
                LoadApartmentManager(getidMain.ToString());
            }
            return View();
        }
        [HttpGet]
        public ActionResult themchungcu()
        {
            if (Session["dangnhap"] == null)
                return RedirectToAction("login", "home");
            var getid = Url.RequestContext.RouteData.Values["id"];
            if (getid == null)
                return RedirectToAction("toanha");
            else if (Int32.Parse(getid.ToString()) == 0)
                ViewData["nameType"] = "Chung cư";
            else
            {
                var get = from cc in db.apartments
                          join type in db.apartments_types on (cc.typeApartment + 1) equals type.idType
                          where Equals(cc.idMain, getid)
                          select type;
                foreach (var type in get)
                {
                    ViewData["nameType"] = type.name_type;
                }
            }
            DropDownListStatusGeneral();
            DropDownListTypeApartment();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult themchungcu(FormCollection collection, apartment chungcu)
        {
            if (Session["dangnhap"] == null)
                return RedirectToAction("login", "home");
            var getid = Url.RequestContext.RouteData.Values["id"];
            DropDownListStatusGeneral();
            DropDownListTypeApartment();
            var ten = collection["name"];
            var mota = collection["description"];
            var trangthai = collection["idStatus"];
            var rent = collection["rent"];
            var daterent = collection["daterent"];
            var exprent = collection["exprent"];
            var dt = collection["DienTich"];
            int typeA = 0;
            if (Int32.Parse(getid.ToString()) == 0)
                ViewData["nameType"] = "Chung cư";
            else
            {
                var getType = from cc in db.apartments
                              join type in db.apartments_types on (cc.typeApartment + 1) equals type.idType
                              where Equals(cc.idMain, getid)
                              select type;
                foreach (var get in getType)
                {
                    typeA = get.idType;
                    ViewData["nameType"] = get.name_type;
                }
            }
            if (String.IsNullOrEmpty(ten))
                ViewData["Err1"] = "(*) Tên không để trống !";
            else if (String.IsNullOrEmpty(trangthai))
                ViewData["Err1"] = "(*) Trạng thái không để trống !";
            else
            {
                if (getid == null)
                    return RedirectToAction("toanha");
                else if (Int32.Parse(getid.ToString()) == 0)
                {
                    chungcu.idSub = 0;
                    chungcu.typeApartment = 1;
                    chungcu.statusGeneral = Int32.Parse(trangthai);
                    chungcu.DienTich = 0;
                    chungcu.name = ten;
                    if (!String.IsNullOrEmpty(mota))
                        chungcu.description = mota;
                    db.apartments.InsertOnSubmit(chungcu);
                    db.SubmitChanges();
                    ViewData["Reg1"] = "(*) Thêm thành công " + ten;
                    return View();
                }
                else
                {
                    if (typeA != 4)
                        chungcu.DienTich = 0;
                    else if (String.IsNullOrEmpty(dt) && typeA == 4)
                    {
                        ViewData["Err1"] = "(*) Diện tích không để trống !";
                        return View();
                    }
                    else if (!String.IsNullOrEmpty(dt) && typeA == 4)
                        chungcu.DienTich = Int32.Parse(dt.ToString());
                    chungcu.idSub = Int32.Parse(getid.ToString());
                    chungcu.typeApartment = typeA;
                    chungcu.statusGeneral = Int32.Parse(trangthai);
                    chungcu.name = ten;
                    if (String.IsNullOrEmpty(daterent))
                        chungcu.DateRent = null;
                    else chungcu.DateRent = DateTime.Parse(daterent);
                    if (String.IsNullOrEmpty(exprent))
                        chungcu.ExpRent = null;
                    else chungcu.ExpRent = DateTime.Parse(daterent).AddDays(Double.Parse(exprent));
                    if (String.IsNullOrEmpty(rent))
                        chungcu.Rent = null;
                    else chungcu.Rent = Decimal.Parse(rent);
                    if (!String.IsNullOrEmpty(mota))
                        chungcu.description = mota;
                    db.apartments.InsertOnSubmit(chungcu);
                    db.SubmitChanges();
                    ViewData["Reg1"] = "(*) Thêm thành công " + ten;

                    //Đăng ký dịch vụ điện, nước tự động
                    var a = from b in db.statusGenerals
                            join c in db.apartments on b.idStatus equals c.statusGeneral
                            where Equals(c.typeApartment, 4) //4 là Home
                            select b;
                    foreach (var d in a)
                    {
                        if (Equals(d.idGroup, 1))
                        {
                            for (int i = 1; i < 3; i++)
                            {
                                service e = new service();
                                e.RegDate = DateTime.Parse(DateTime.Now.ToString());
                                e.ExpDate = DateTime.Parse(DateTime.Now.ToString()).AddDays(30);
                                e.serviceType = i; // Dịch vụ điện, nước
                                e.idHome = chungcu.idMain;
                                e.value = 0;
                                db.services.InsertOnSubmit(e);
                                db.SubmitChanges();
                            }
                        }
                        break;
                    }
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult quanlydichvu()
        {
            if (Session["dangnhap"] == null)
                return RedirectToAction("login", "home");

            DropDownHeSo();
            Quanlydichvu mymodel = new Quanlydichvu();
            mymodel.apartment = LoadRentHouse();
            mymodel.services_type = DSDichVu();
            mymodel.Dichvukhachhang = DVKhachHang();
            return View(mymodel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult quanlydichvu(FormCollection collection)
        {
            if (Session["dangnhap"] == null)
                return RedirectToAction("login", "home");

            DropDownHeSo();
            Quanlydichvu mymodel = new Quanlydichvu();
            mymodel.apartment = LoadRentHouse();
            mymodel.services_type = DSDichVu();
            mymodel.Dichvukhachhang = DVKhachHang();
            View(mymodel);
            var tendichvu = collection["name_service"];
            var mota = collection["description"];
            var gia = collection["Price"];
            var donvi = collection["DonVi"];
            var heso = collection["HeSo"];

            if (mota.ToString().Length > 200)
            {
                ViewData["Err1"] = "(*) Mô tả không vượt quá 200 ký tự !";
                return View();
            }
            if (Equals(heso, 1))
            {
                if (String.IsNullOrEmpty(donvi))
                {
                    ViewData["Err1"] = "(*) Vui lòng nhập đơn vị !";
                    return View();
                }
            }
            if (String.IsNullOrEmpty(tendichvu))
                ViewData["Err1"] = "(*) Vui lòng nhập tên dịch vụ !";
            else if (String.IsNullOrEmpty(mota))
                ViewData["Err1"] = "(*) Vui lòng nhập Mô tả !";
            else if (String.IsNullOrEmpty(gia))
                ViewData["Err1"] = "(*) Vui lòng nhập Giá dịch vụ !";
            else
            {
                services_type add = new services_type();
                add.name_service = tendichvu;
                add.description = mota;
                add.Price = Decimal.Parse(gia);
                if (String.IsNullOrEmpty(donvi))
                    add.DonVi = null;
                else add.DonVi = donvi;
                if (Int32.Parse(heso) == 1)
                    add.type = true;
                else
                    add.type = false;
                db.services_types.InsertOnSubmit(add);
                db.SubmitChanges();
                return RedirectToAction("quanlydichvu");
            }
            return View();
        }
        [HttpGet]
        public ActionResult capnhatdichvukhachhang()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idChucvu"], 1) && !Equals(Session["idChucvu"], 3)) return RedirectToAction("Error404", "home");

            var id = Url.RequestContext.RouteData.Values["id"];
            if (id == null) return RedirectToAction("Error404", "home");
            UpdateDVKhachHang(id.ToString());
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult capnhatdichvukhachhang(FormCollection collection)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idChucvu"], 1) && !Equals(Session["idChucvu"], 3)) return RedirectToAction("Error404", "home");

            var id = Url.RequestContext.RouteData.Values["id"];
            if (id == null) return RedirectToAction("Error404", "home");
            UpdateDVKhachHang(id.ToString());

            var heso = collection["HeSoSuDung"];
            if (String.IsNullOrEmpty(heso))
            {
                ViewData["Err"] = "(*) Vui lòng nhập hệ số sử dụng !";
                return View();
            }

            var query = from a in db.services
                        where Equals(a.idService, id)
                        select a;
            foreach (var b in query)
            {
                if (String.IsNullOrEmpty(heso))
                    b.value = 0;
                else b.value = Int32.Parse(heso);
                ViewData["success"] = "(*) Cập nhật thành công !";
                UpdateDVKhachHang(id.ToString());
            }
            db.SubmitChanges();
            return View();
        }
        [NonAction]
        private ActionResult UpdateDVKhachHang(String getid)
        {
            IList<Dichvukhachhang> infoacc_one = new List<Dichvukhachhang>();
            var query = from a in db.services
                        join b in db.services_types on a.serviceType equals b.serviceType
                        where Equals(a.idService, getid)
                        select new { a, b };
            var infoaccs = query.ToList();
            foreach (var info in infoaccs)
            {
                infoacc_one.Add(new Dichvukhachhang()
                {
                    serviceType = info.a.serviceType,
                    name_service = info.b.name_service,
                    Price = info.b.Price,
                    type = info.b.type,
                    DonVi = info.b.DonVi,
                    RegDate = info.a.RegDate,
                    ExpDate = info.a.ExpDate,
                    value = float.Parse(info.a.value.ToString()),
                    idHome = info.a.idHome,
                    idService = info.a.idService
                });
            }
            return View(infoacc_one);
        }
        [HttpGet]
        public ActionResult capnhatdichvu()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");

            DropDownHeSo();
            var id = Url.RequestContext.RouteData.Values["id"];
            if (id != null)
                CapnhatDV(id.ToString());
            else return RedirectToAction("Error404", "home");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult capnhatdichvu(FormCollection collection)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");
            var id = Url.RequestContext.RouteData.Values["id"];
            if (id != null)
            {
                DropDownHeSo();
                CapnhatDV(id.ToString());
                var tendv = collection["name_service"];
                var description = collection["description"];
                var gia = collection["Price"];
                var donvi = collection["DonVi"];
                var dangdichvu = collection["HeSo"];

                if (String.IsNullOrEmpty(tendv))
                    ViewData["Err"] = "(*) Vui lòng nhập tên dịch vụ !";
                else if (String.IsNullOrEmpty(gia))
                    ViewData["Err"] = "(*) Vui lòng nhập giá dịch vụ !";
                else if (String.IsNullOrEmpty(dangdichvu))
                    ViewData["Err"] = "(*) Vui lòng chọn dạng dịch vụ !";
                else
                {
                    var a = from b in db.services_types
                            where Equals(b.serviceType, id)
                            select b;
                    foreach (var c in a)
                    {
                        c.name_service = tendv;
                        c.description = description;
                        c.Price = decimal.Parse(gia);
                        c.DonVi = donvi;
                        if (Int32.Parse(dangdichvu) == 1)
                            c.type = true;
                        else
                            c.type = false;
                        db.SubmitChanges();
                        ViewData["Success"] = "(*) Cập nhật thành công !";
                        DropDownHeSo();
                        CapnhatDV(id.ToString());
                    }
                }
            }
            else return RedirectToAction("Error404", "home");
            return View();
        }
        [HttpGet]
        public ActionResult dangkydichvu()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            var id = Url.RequestContext.RouteData.Values["id"];
            if (id == null || Session["DKDVidHome"] == null) return RedirectToAction("Error404", "home");
            try
            {
                var check = from b in db.services
                            where Equals(b.idHome, Session["DKDVidHome"]) && Equals(b.serviceType, id)
                            select b;
                foreach (var m in check)
                {
                    LoadDichVu();
                    ViewData["Err1"] = "(*) Dịch vụ này đã được đăng ký nên không thể đăng ký nữa !";
                    return View();
                }
                service a = new service();
                a.RegDate = DateTime.Parse(DateTime.Now.ToString());
                a.ExpDate = DateTime.Parse(DateTime.Now.ToString()).AddDays(30);
                a.serviceType = Int32.Parse(id.ToString()); // id Service
                a.idHome = Int32.Parse(Session["DKDVidHome"].ToString());
                a.value = 0;
                db.services.InsertOnSubmit(a);
                db.SubmitChanges();
                LoadDichVu();
                ViewData["Success"] = "(*) Đăng ký dịch vụ thành công !";
                ViewData["Thongtin1"] = "Mã căn hộ: " + Session["DKDVidHome"];
                ViewData["Thongtin2"] = "Ngày đăng ký: " + a.RegDate.ToString("dd/MM/yyyy");
                ViewData["Thongtin3"] = "Ngày hết hạn: " + a.ExpDate.ToString("dd/MM/yyyy");

                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/SendMail/RegService.html"));
                content = content.Replace("{{idHome}}", Session["DKDVidHome"].ToString());
                content = content.Replace("{{idService}}", a.idService.ToString());
                var k = from j in db.services_types
                        where Equals(j.serviceType, id)
                        select j.name_service;
                foreach (var x in k)
                    content = content.Replace("{{nameService}}", x);
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendMail(toEmail, "[No Reply] Đăng ký dịch vụ", content); //Gửi mail tới ng quản trị test

                //Gửi đến Email khách hàng
                var d = from g in db.logins
                        join h in db.owner_homes on g.idUser equals h.idUser
                        where Equals(h.idHome, Session["DKDVidHome"])
                        select g.Email;
                foreach (var s in d)
                    if (s != null)
                        new MailHelper().SendMail(s, "[No Reply] Đăng ký dịch vụ", content);

                Session.Remove("DKDVidHome");
                return View();
            }
            catch
            {
                ViewData["Err1"] = "(*) Đã xảy ra lỗi vui lòng đăng ký lại !";
                return View();
            }
        }
        [HttpGet]
        public ActionResult dichvu()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            LoadDichVu();
            var id = Url.RequestContext.RouteData.Values["id"];
            if (id == null) return RedirectToAction("Error404", "home");

            if (!Equals(Session["idLevel"], 0)) return View();
            var a = (from b in db.owner_homes
                     where Equals(b.idUser, Session["dangnhap"]) && Equals(b.idHome, id)
                     select b).Count();
            if (a != 0)
            {
                Session["DKDVidHome"] = id;
                return View();
            }
            return RedirectToAction("Error404", "home");
        }
        [HttpGet]
        public ActionResult canho()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            var id = Url.RequestContext.RouteData.Values["id"];
            if (id == null) return RedirectToAction("Error404", "home");

            Canho mymodel = new Canho();
            mymodel.ApartmentManager = ThongTinCanHo(id.ToString());
            mymodel.InfoAccount = LoadUserHome(id.ToString());
            mymodel.Dichvukhachhang = LoadDichVuHome(id.ToString());
            mymodel.LogDichvu = LogDichvu(id.ToString());

            if (!Equals(Session["idLevel"], 0)) return View(mymodel);
            var a = (from b in db.owner_homes
                     where Equals(b.idUser, Session["dangnhap"]) && Equals(b.idHome, id)
                     select b).Count();
            if (a != 0) return View(mymodel);

            return RedirectToAction("Error404", "home");
        }
        [NonAction]
        private List<InfoAccount> LoadUserHome(string getIDhome)
        {
            List<InfoAccount> LoadUserHome = new List<InfoAccount>();

            var query = from acc in db.accounts
                        join lg in db.logins on acc.idUser equals lg.idUser
                        join cv in db.positions on acc.idStaff equals cv.idStaff
                        join own in db.owner_homes on acc.idUser equals own.idUser
                        where Equals(own.idHome, getIDhome)
                        select new { acc, lg, cv, own };
            foreach (var info in query)
            {
                LoadUserHome.Add(new InfoAccount()
                {
                    idUser = info.acc.idUser,
                    Username = info.acc.Username,
                    Email = info.lg.Email,
                    PhoneNumber = info.lg.PhoneNumber,
                    idHome = Int32.Parse(info.own.idHome.ToString()),
                    IDCard = info.acc.IDCard,
                    Birthday = DateTime.Parse(info.acc.Birthday.ToString()),
                    RegDate = DateTime.Parse(info.acc.RegDate.ToString()),
                    lastLogin = DateTime.Parse(info.acc.lastLogin.ToString()),
                    Sex = info.acc.Sex,
                    Image_Avatar = info.acc.Image_Avatar,
                    Image_IDCard1 = info.acc.Image_IDCard1,
                    Image_IDCard2 = info.acc.Image_IDCard2
                });
            }
            return LoadUserHome;
        }
        [NonAction]
        private List<Dichvukhachhang> LoadDichVuHome(string getIDhome)
        {
            List<Dichvukhachhang> LoadDichVuHome = new List<Dichvukhachhang>();

            var query = from a in db.services
                        join b in db.services_types on a.serviceType equals b.serviceType
                        where Equals(a.idHome, getIDhome)
                        select new { a, b };
            foreach (var info in query)
            {
                LoadDichVuHome.Add(new Dichvukhachhang()
                {
                    name_service = info.b.name_service,
                    idService = info.a.idService,
                    RegDate = info.a.RegDate,
                    ExpDate = info.a.ExpDate,
                    type = info.b.type,
                    value = float.Parse(info.a.value.ToString()),
                    DonVi = info.b.DonVi,
                    Price = info.b.Price
                });
            }
            return LoadDichVuHome;
        }
        [NonAction]
        private List<log_service> LogDichvu(string getIDhome)
        {
            List<log_service> LogDichvu = new List<log_service>();

            var query = from a in db.log_services
                        where Equals(a.idHome, getIDhome) && a.CancelOrSave == true
                        select a;
            foreach (var info in query)
            {
                LogDichvu.Add(new log_service()
                {
                    nameService = info.nameService,
                    RegDate = info.RegDate,
                    ExpDate = info.ExpDate,
                    HeSo = info.HeSo,
                    Price = info.Price,
                    DonVi = info.DonVi,
                    type = info.type
                });
            }
            return LogDichvu;
        }
        [NonAction]
        private ActionResult CapnhatDV(String getid)
        {
            IList<services_type> CapnhatDV = new List<services_type>();
            var query = from b in db.services_types
                        where Equals(b.serviceType, getid)
                        select b;

            var a = query.ToList();
            foreach (var info in a)
            {
                CapnhatDV.Add(new services_type()
                {
                    serviceType = info.serviceType,
                    name_service = info.name_service,
                    description = info.description,
                    Price = info.Price,
                    type = info.type,
                    DonVi = info.DonVi
                });
            }
            return View(CapnhatDV);
        }
        [NonAction]
        private ActionResult LoadChitietcanho(String getidUser)
        {
            IList<owner_home> infoacc_one = new List<owner_home>();
            var query = from a in db.owner_homes
                        where Equals(a.idUser, getidUser)
                        select a;

            var infoaccs = query.ToList();
            foreach (var info in infoaccs)
            {
                infoacc_one.Add(new owner_home()
                {
                    idHome = info.idHome
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
                    IDCard = info.acc.IDCard,
                    Birthday = DateTime.Parse(info.acc.Birthday.ToString()),
                    RegDate = DateTime.Parse(info.acc.RegDate.ToString()),
                    lastLogin = DateTime.Parse(info.acc.lastLogin.ToString()),
                    Sex = info.acc.Sex,
                    Image_Avatar = info.acc.Image_Avatar,
                    Image_IDCard1 = info.acc.Image_IDCard1,
                    Image_IDCard2 = info.acc.Image_IDCard2
                });
            }
            return View(infoacc_one);
        }
        [NonAction]
        private void DropDownListStatusGeneral()
        {
            var dataList = new SelectList(
                            (
                                from status in db.statusGenerals
                                where status.idGroup != 2
                                select new SelectListItem { Text = status.nameStatus, Value = status.idStatus.ToString() }
                            ), "Value", "Text");
            ViewBag.LoadStatusGeneral = dataList;
        }
        [NonAction]
        private void DropDownListTypeApartment()
        {
            var dataList = new SelectList(
                            (
                                from type in db.apartments_types
                                select new SelectListItem { Text = type.name_type, Value = type.idType.ToString() }
                            ), "Value", "Text");
            ViewBag.LoadTypeApartment = dataList;
        }
        [NonAction]
        private ActionResult LoadApartmentManager(string id)
        {
            IList<ApartmentManager> apartmentmanager = new List<ApartmentManager>();

            var query = from apartment in db.apartments
                        join status in db.statusGenerals on apartment.statusGeneral equals status.idStatus
                        join type in db.apartments_types on apartment.typeApartment equals type.idType
                        where Equals(apartment.typeApartment, id) || Equals(apartment.idMain, id)
                        select new { apartment, status, type };
            var manager = query.ToList();
            foreach (var info in manager)
            {
                apartmentmanager.Add(new ApartmentManager()
                {
                    idMain = info.apartment.idMain,
                    name = info.apartment.name,
                    statusGeneral = Int32.Parse(info.apartment.statusGeneral.ToString()),
                    description = info.apartment.description,
                    nameStatus = info.status.nameStatus,
                    idGroup = info.status.idGroup,
                    name_type = info.type.name_type,
                    typeApartment = info.type.idType,
                    idSub = info.apartment.idSub,
                    Rent = info.apartment.Rent,
                    DateRent = info.apartment.DateRent,
                    ExpRent = info.apartment.ExpRent,
                    DienTich = info.apartment.DienTich.Value
                });
            }
            return View(apartmentmanager);
        }
        [NonAction]
        private List<apartment> CanHoQuaHan()
        {
            List<apartment> CanHoQuaHan = new List<apartment>();
            CanHoQuaHan = db.apartments.Where(ax => ax.ExpRent.Value.Date < DateTime.Now.Date).ToList();
            return CanHoQuaHan;
        }
        [NonAction]
        private List<apartment> CHGanHetHan()
        {
            List<apartment> CHGanHetHan = new List<apartment>();
            CHGanHetHan = db.apartments.Where(ax => (DateTime.Now.Date - ax.ExpRent.Value.Date).TotalDays > -4 && (DateTime.Now.Date - ax.ExpRent.Value.Date).TotalDays < 0).ToList();
            return CHGanHetHan;
        }
        [NonAction]
        private List<Dichvukhachhang> DichVuQuaHan()
        {
            List<Dichvukhachhang> DichVuQuaHan = new List<Dichvukhachhang>();

            var query = from a in db.services
                        join b in db.services_types on a.serviceType equals b.serviceType
                        where a.ExpDate.Date < DateTime.Now.Date
                        select new { a, b };
            foreach (var info in query)
            {
                DichVuQuaHan.Add(new Dichvukhachhang()
                {
                    serviceType = info.a.serviceType,
                    name_service = info.b.name_service,
                    Price = info.b.Price,
                    type = info.b.type,
                    DonVi = info.b.DonVi,
                    RegDate = info.a.RegDate,
                    ExpDate = info.a.ExpDate,
                    value = float.Parse(info.a.value.ToString()),
                    idHome = info.a.idHome,
                    idService = info.a.idService
                });
            }
            return DichVuQuaHan;
        }
        [NonAction]
        private List<Dichvukhachhang> DichVuGanHetHan()
        {
            List<Dichvukhachhang> DichVuGanHetHan = new List<Dichvukhachhang>();

            var query = from a in db.services
                        join b in db.services_types on a.serviceType equals b.serviceType
                        where (DateTime.Now.Date - a.ExpDate.Date).TotalDays > -4 && (DateTime.Now.Date - a.ExpDate.Date).TotalDays < 0
                        select new { a, b };
            foreach (var info in query)
            {
                DichVuGanHetHan.Add(new Dichvukhachhang()
                {
                    serviceType = info.a.serviceType,
                    name_service = info.b.name_service,
                    Price = info.b.Price,
                    type = info.b.type,
                    DonVi = info.b.DonVi,
                    RegDate = info.a.RegDate,
                    ExpDate = info.a.ExpDate,
                    value = float.Parse(info.a.value.ToString()),
                    idHome = info.a.idHome,
                    idService = info.a.idService
                });
            }
            return DichVuGanHetHan;
        }
        [NonAction]
        private List<apartment> LoadRentHouse()
        {
            List<apartment> LoadRentHouse = new List<apartment>();

            var query = from apartment in db.apartments
                        where apartment.statusGeneral == 3
                        select apartment;

            foreach (var info in query)
            {
                LoadRentHouse.Add(new apartment()
                {
                    idMain = info.idMain,
                    name = info.name,
                    statusGeneral = info.statusGeneral,
                    Rent = info.Rent,
                    DateRent = info.DateRent,
                    ExpRent = info.ExpRent
                });
            }
            return LoadRentHouse;
        }
        [NonAction]
        private List<Dichvukhachhang> DVKhachHang()
        {
            List<Dichvukhachhang> DVKhachHang = new List<Dichvukhachhang>();

            var query = from a in db.services
                        join b in db.services_types on a.serviceType equals b.serviceType
                        select new { a, b };
            foreach (var info in query)
            {
                DVKhachHang.Add(new Dichvukhachhang()
                {
                    serviceType = info.a.serviceType,
                    name_service = info.b.name_service,
                    Price = info.b.Price,
                    type = info.b.type,
                    DonVi = info.b.DonVi,
                    RegDate = info.a.RegDate,
                    ExpDate = info.a.ExpDate,
                    value = float.Parse(info.a.value.ToString()),
                    idHome = info.a.idHome,
                    idService = info.a.idService
                });
            }
            return DVKhachHang;
        }
        [NonAction]
        private List<ApartmentManager> ThongTinCanHo(string id)
        {
            List<ApartmentManager> ThongTinCanHo = new List<ApartmentManager>();
            var query = from a in db.apartments
                        join b in db.statusGenerals on a.statusGeneral equals b.idStatus
                        where Equals(a.idMain, id)
                        select new { a, b };
            foreach (var info in query)
            {
                ThongTinCanHo.Add(new ApartmentManager()
                {
                    idMain = info.a.idMain,
                    nameStatus = info.b.nameStatus,
                    Rent = info.a.Rent,
                    idGroup = info.b.idGroup,
                    statusGeneral = info.a.statusGeneral,
                    DienTich = info.a.DienTich.Value
                });
            }
            return ThongTinCanHo;
        }
        [NonAction]
        private List<services_type> DSDichVu()
        {
            List<services_type> DSDichVu = new List<services_type>();

            var query = from a in db.services_types
                        select a;
            foreach (var info in query)
            {
                DSDichVu.Add(new services_type()
                {
                    serviceType = info.serviceType,
                    name_service = info.name_service,
                    description = info.description,
                    Price = info.Price,
                    type = info.type,
                    DonVi = info.DonVi
                });
            }
            return DSDichVu;
        }
        [NonAction]
        private ActionResult LoadDichVu() //Load Block, Floor, Home
        {
            IList<services_type> list = new List<services_type>();

            var query = from a in db.services_types
                        select a;
            var listservice = query.ToList();
            foreach (var info in listservice)
            {
                list.Add(new services_type()
                {
                    serviceType = info.serviceType,
                    name_service = info.name_service,
                    description = info.description,
                    type = info.type,
                    Price = info.Price,
                    DonVi = info.DonVi
                });
            }
            return View(list);
        }
        [NonAction]
        private ActionResult LoadApartmentManagers(int id, int idtype) //Load Block, Floor, Home
        {
            IList<ApartmentManager> apartmentmanagers = new List<ApartmentManager>();

            var query = from apartment in db.apartments
                        join status in db.statusGenerals on apartment.statusGeneral equals status.idStatus
                        join type in db.apartments_types on apartment.typeApartment equals type.idType
                        where Equals(apartment.typeApartment, idtype) && Equals(apartment.idSub, id)
                        orderby apartment.name ascending
                        select new { apartment, status, type };
            var manager = query.ToList();
            foreach (var info in manager)
            {
                apartmentmanagers.Add(new ApartmentManager()
                {
                    idMain = info.apartment.idMain,
                    name = info.apartment.name,
                    statusGeneral = Int32.Parse(info.apartment.statusGeneral.ToString()),
                    description = info.apartment.description,
                    nameStatus = info.status.nameStatus,
                    idGroup = info.status.idGroup,
                    name_type = info.type.name_type,
                    typeApartment = info.type.idType
                });
            }
            return View(apartmentmanagers);
        }
        [NonAction]
        private ActionResult DanhSachDichVuDaHuy()
        {
            IList<Dichvukhachhang> DanhSachDichVuDaHuy = new List<Dichvukhachhang>();

            var query = from a in db.log_services
                        join b in db.log_reasonCancels on a.idLogService equals b.idLog_Service
                        where a.CancelOrSave == false
                        select new { a, b };
            var DSDV = query.ToList();
            foreach (var info in DSDV)
            {
                DanhSachDichVuDaHuy.Add(new Dichvukhachhang()
                {
                    name_service = info.a.nameService,
                    RegDate = info.a.RegDate,
                    ExpDate = info.a.ExpDate,
                    value = float.Parse(info.a.HeSo.ToString()),
                    Price = info.a.Price,
                    DonVi = info.a.DonVi,
                    type = info.a.type,
                    idHome = info.a.idHome,
                    CancelOrSave = false,
                    Reason = info.b.Reason,
                    idUserCancel = info.a.idUserCancel
                });
            }
            return View(DanhSachDichVuDaHuy);
        }
        [NonAction]
        private void DropDownHeSo()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string Textt;
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) Textt = "Không hệ số";
                else Textt = "Hệ số";
                list.Add(new SelectListItem()
                {
                    Text = Textt,
                    Value = i.ToString()
                });
            }
            ViewBag.LoadHeSo = list;
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