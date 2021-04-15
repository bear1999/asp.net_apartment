using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _NSY.Models;
using System.IO;
using System.Linq.Expressions;

namespace _NSY.Controllers
{
    public class adminController : Controller
    {
        ApartmentDataContext db = new ApartmentDataContext();
        // GET: admin
        [HttpGet]
        public ActionResult EditInfoBank()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");
            var getid = Url.RequestContext.RouteData.Values["id"];
            if (getid == null) return RedirectToAction("Error404", "home");
            return View(db.InfoBanks.Where(ax => ax.idInfoBank.Equals(getid)));
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditInfoBank(FormCollection collection)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");
            var getid = Url.RequestContext.RouteData.Values["id"];
            if (getid == null) return RedirectToAction("Error404", "home");
            var ten = collection["nameBank"];
            var stk = collection["numberBank"];
            var sohuu = collection["ownerBank"];
            var chinhanh = collection["ChiNhanhBank"];

            if (ten.Length > 60)
            {
                ViewData["Err"] = "(*) Tên ngân hàng không vượt quá 60 ký tự !";
                return View();
            }
            else if (stk.Length > 20)
            {
                ViewData["Err"] = "(*) Số tài khoản không vượt quá 20 ký tự !";
                return View();
            }
            else if (sohuu.Length > 60)
            {
                ViewData["Err"] = "(*) Tên chủ sở hữu không vượt quá 60 ký tự !";
                return View();
            }
            else if (chinhanh.Length > 30)
            {
                ViewData["Err"] = "(*) Tên chi nhánh không vượt quá 30 ký tự !";
                return View();
            }

            if (String.IsNullOrEmpty(ten) || String.IsNullOrEmpty(stk) || String.IsNullOrEmpty(sohuu) || String.IsNullOrEmpty(chinhanh))
            {
                ViewData["Err"] = "(*) Vui lòng nhập đầy đủ thông tin !";
                return View();
            }
            var a = from b in db.InfoBanks
                    where Equals(b.idInfoBank, getid)
                    select b;
            foreach (var c in a)
            {
                c.nameBank = ten;
                c.numberBank = stk;
                c.ownerBank = sohuu;
                c.ChiNhanhBank = chinhanh;
            }
            db.SubmitChanges();
            ViewData["Err"] = "(*) Cập nhật thành công !";
            return View(db.InfoBanks.Where(ax => ax.idInfoBank.Equals(getid)));
        }
        [HttpGet]
        public ActionResult DeleteInfoBank()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");
            var getid = Url.RequestContext.RouteData.Values["id"];
            if (getid == null) return RedirectToAction("Error404", "home");
            var a = from b in db.InfoBanks
                    where Equals(b.idInfoBank, getid)
                    select b;
            foreach (var d in a)
                db.InfoBanks.DeleteOnSubmit(d);
            db.SubmitChanges();
            return RedirectToAction("InfoBank", "admin");
        }
        [HttpGet]
        public ActionResult InfoBank()
        {
            if(Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");
            LoadInfoBank();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult InfoBank(FormCollection collection)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");
            LoadInfoBank();
            var ten = collection["nameBank"];
            var stk = collection["numberBank"];
            var sohuu = collection["ownerBank"];
            var chinhanh = collection["ChiNhanhBank"];

            if (ten.Length > 60)
            {
                ViewData["Err"] = "(*) Tên ngân hàng không vượt quá 60 ký tự !";
                return View();
            }
            else if(stk.Length > 20)
            {
                ViewData["Err"] = "(*) Số tài khoản không vượt quá 20 ký tự !";
                return View();
            }
            else if (sohuu.Length > 60)
            {
                ViewData["Err"] = "(*) Tên chủ sở hữu không vượt quá 60 ký tự !";
                return View();
            }
            else if (chinhanh.Length > 30)
            {
                ViewData["Err"] = "(*) Tên chi nhánh không vượt quá 30 ký tự !";
                return View();
            }

            if (String.IsNullOrEmpty(ten) || String.IsNullOrEmpty(stk) || String.IsNullOrEmpty(sohuu) || String.IsNullOrEmpty(chinhanh))
            {
                ViewData["Err"] = "(*) Vui lòng nhập đầy đủ thông tin !";
                return View();
            }

            InfoBank bank = new InfoBank();
            bank.nameBank = ten;
            bank.numberBank = stk;
            bank.ownerBank = sohuu;
            bank.ChiNhanhBank = chinhanh;
            db.InfoBanks.InsertOnSubmit(bank);
            db.SubmitChanges();
            return RedirectToAction("InfoBank", "admin");
        }
        [HttpGet]
        public ActionResult EditContact()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");
            LoadInfoApartment();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, ActionName("EditContact")]
        public ActionResult EditContact(FormCollection collection, HttpPostedFileBase image)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");
            LoadInfoApartment();

            var name = collection["nameApartment"];
            var addres = collection["addressApartment"];
            var email = collection["emailApartment"];
            var tel = collection["telApartment"];
            var tittle = collection["tittleNavbar"];

            if (name.Length > 30)
            {
                ViewData["Err1"] = "(*) Tên chung cư không vượt quá 30 ký tự";
                return View();
            }
            else if (addres.Length > 50)
            {
                ViewData["Err1"] = "(*) Địa chỉ chung cư không vượt quá 50 ký tự";
                return View();
            }
            else if (tel.Length > 15)
            {
                ViewData["Err1"] = "(*) Số điện thoại chung cư không vượt quá 15 ký tự";
                return View();
            }
            else if (email.Length > 40)
            {
                ViewData["Err1"] = "(*) Email chung cư không vượt quá 40 ký tự";
                return View();
            }
            else if (tittle.Length > 15)
            {
                ViewData["Err1"] = "(*) Tiêu đề chung cư không vượt quá 15 ký tự";
                return View();
            }

            var d = from b in db.InfoApartments
                    select b;
            foreach (var cd in d)
            {
                cd.nameApartment = name;
                cd.addressApartment = addres;
                cd.emailApartment = email;
                cd.telApartment = tel;
                cd.tittleNavbar = tittle;

                if (ModelState.IsValid)
                {
                    if (image != null)
                    {
                        string extension1 = Path.GetExtension(image.FileName);
                        if (!Equals(extension1, ".png") && !Equals(extension1, ".jpg") && !Equals(extension1, ".gif"))
                        {
                            ViewData["Err1"] = "(*) Chỉ được cập nhật định dạng hình ảnh (.jpg | .png | .gif)";
                            return View();
                        }
                        var filename = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(image.FileName);
                        var path = Path.Combine(Server.MapPath("~/Assets/dist/img"), filename);
                        if (System.IO.File.Exists(path))
                            ViewData["Err1"] = "(*) Hình ảnh đã tồn tại !";
                        else
                        {
                            if (cd.imageNavbar != null)
                                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/dist/img"), cd.imageNavbar));
                            image.SaveAs(path);
                            cd.imageNavbar = filename;
                        }
                    }
                }
                ViewBag.Success = "(*) Cập nhật thành công !";
            }
            db.SubmitChanges();
            return View();
        }
        [HttpGet]
        public ActionResult themnhanvien()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");
            DropDownListChucVu();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult themnhanvien(FormCollection collection, account dangky, login dangky1, HttpPostedFileBase fileAvatar, HttpPostedFileBase fileIDCard1, HttpPostedFileBase fileIDCard2)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");

            DropDownListChucVu();
            var tendn = collection["Username"];
            var ngaysinh = String.Format("{0:MM/dd/yyy}", collection["Birthday"]);
            var gt = collection["Sex"];
            var sdt = collection["PhoneNumber"];
            var cmnd = collection["IDCard"];
            var email = collection["Email"];
            var mk = collection["Password"];
            var mk1 = collection["Password1"];
            var chucvu = collection["idStaff"];
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
                dangky.Username = tendn;
                dangky.Birthday = DateTime.Parse(ngaysinh);
                dangky.Sex = gt;
                dangky.IDCard = cmnd;
                dangky.idStaff = Int32.Parse(chucvu);
                dangky.RegDate = DateTime.Parse(DateTime.Now.ToString());
                dangky.lastLogin = DateTime.Parse(DateTime.Now.ToString());
                dangky.Image_Avatar = null;

                if (ModelState.IsValid)
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
                db.accounts.InsertOnSubmit(dangky);
                db.SubmitChanges();
                //-----
                owner_home owner = new owner_home();
                owner.idHome = 0;
                owner.idUser = dangky.idUser;
                db.owner_homes.InsertOnSubmit(owner);
                db.SubmitChanges();
                //-----
                var getIDUser = from getid in db.accounts
                                join cv in db.positions on getid.idStaff equals cv.idStaff
                                where Equals(getid.IDCard.ToString(), cmnd.ToString())
                                select new { getid.idUser, cv.name_position };

                foreach (var id in getIDUser)
                {
                    dangky1.PhoneNumber = sdt;
                    dangky1.Email = email;
                    dangky1.Password = GetMD5(mk);
                    dangky1.idUser = id.idUser;
                    db.logins.InsertOnSubmit(dangky1);
                    db.SubmitChanges();
                    ViewData["Reg1"] = "Mã nhân viên: " + id.idUser;
                    ViewData["Reg2"] = "Nhân viên: " + tendn;
                    ViewData["Reg3"] = "Chức vụ: " + id.name_position;
                }
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult quanlynhanvien()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");

            IList<InfoAccount> infoacc = new List<InfoAccount>();
            var query = from acc in db.accounts
                        join lg in db.logins on acc.idUser equals lg.idUser
                        join cv in db.positions on acc.idStaff equals cv.idStaff
                        where acc.idStaff != 0
                        orderby acc.RegDate descending
                        select new { acc, lg, cv };

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
                    name_position = info.cv.name_position
                });
            }
            return View(infoacc);
        }
        [HttpGet]
        public ActionResult capnhatnhanvien()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");

            DropDownListChucVu();
            var getidUser = Url.RequestContext.RouteData.Values["id"];
            Load2Models(getidUser.ToString());
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult capnhatnhanvien(FormCollection collection, HttpPostedFileBase fileAvatar, HttpPostedFileBase fileIDCard1, HttpPostedFileBase fileIDCard2, string updateAccount)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 2)) return RedirectToAction("Error404", "home");

            var getidUser = Url.RequestContext.RouteData.Values["id"];
            DropDownListChucVu();
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
                var chucvu = collection["idStaff"];
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
                else if (String.IsNullOrEmpty(sdt))
                    ViewData["Err1"] = "(*) Số điện thoại không để trống !";
                else if (String.IsNullOrEmpty(cmnd))
                    ViewData["Err1"] = "(*) Thẻ căn cước / CMND không để trống !";
                else if (String.IsNullOrEmpty(email))
                    ViewData["Err1"] = "(*) Địa chỉ Email không để trống !";
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
                            info.acc.Username = tendn;
                            info.acc.Birthday = DateTime.Parse(ngaysinh);
                            info.acc.Sex = gt;
                            info.acc.IDCard = cmnd;
                            info.lg.PhoneNumber = sdt;
                            info.lg.Email = email;
                            if (!String.IsNullOrEmpty(chucvu))
                                info.acc.idStaff = Int32.Parse(chucvu);
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
                if (Session["dangnhap"].ToString() == getidUser.ToString())
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
                        db.accounts.DeleteOnSubmit(del.acc);
                        db.logins.DeleteOnSubmit(del.lg);
                        db.owner_homes.DeleteOnSubmit(del.own);
                        if (del.acc.Image_Avatar != null)
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Image"), del.acc.Image_Avatar));
                        if (del.acc.Image_IDCard1 != null)
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Image"), del.acc.Image_IDCard1));
                        if (del.acc.Image_IDCard2 != null)
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Image"), del.acc.Image_IDCard2));
                    }
                    db.SubmitChanges();
                    return RedirectToAction("quanlynhanvien");
                }
            }
            return View();
        }
        [NonAction]
        private ActionResult LoadInfoBank()
        {
            IList<InfoBank> infoacc_one = new List<InfoBank>();
            var query = from a in db.InfoBanks
                        select a;
            var infoaccs = query.ToList();
            foreach (var info in infoaccs)
            {
                infoacc_one.Add(new InfoBank()
                {
                    nameBank = info.nameBank,
                    numberBank = info.numberBank,
                    ownerBank = info.ownerBank,
                    ChiNhanhBank = info.ChiNhanhBank
                });
            }
            return View(infoaccs);
        }
        [NonAction]
        private ActionResult LoadInfoApartment()
        {
            IList<InfoApartment> infoacc_one = new List<InfoApartment>();
            var query = from a in db.InfoApartments
                        select a;
            var infoaccs = query.ToList();
            foreach (var info in infoaccs)
            {
                infoacc_one.Add(new InfoApartment()
                {
                    nameApartment = info.nameApartment,
                    addressApartment = info.addressApartment,
                    telApartment = info.telApartment,
                    emailApartment = info.emailApartment,
                    tittleNavbar = info.tittleNavbar
                });
            }
            return View(infoaccs);
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
                    Image_IDCard1 = info.acc.Image_IDCard1,
                    Image_IDCard2 = info.acc.Image_IDCard2
                });
            }
            return View(infoacc_one);
        }
        [NonAction]
        private ActionResult LoadLogoName()
        {
            IList<InfoApartment> infoacc_one = new List<InfoApartment>();
            var query = from a in db.InfoApartments
                        select a;

            var infoaccs = query.ToList();
            foreach (var info in infoaccs)
            {
                infoacc_one.Add(new InfoApartment()
                {
                    nameApartment = info.nameApartment,
                    addressApartment = info.addressApartment,
                    telApartment = info.telApartment,
                    emailApartment = info.emailApartment,
                    tittleNavbar = info.tittleNavbar,
                    imageNavbar = info.imageNavbar
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