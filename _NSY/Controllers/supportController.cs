using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _NSY.Models;

namespace _NSY.Controllers
{
    public class supportController : Controller
    {
        ApartmentDataContext db = new ApartmentDataContext();
        // GET: support
        [HttpGet]
        public ActionResult danhsachhotro()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            Hotro model = new Hotro();
            model.Kinhdoanh = LoadTicketKinhDoanh();
            model.Quanly = LoadTicketQuanLy();
            model.KinhdoanhHT = LoadTicketKinhDoanhHT();
            model.QuanlyHT = LoadTicketQuanLyHT();
            return View(model);
        }
        [HttpGet]
        public ActionResult yeucauhotro()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            Hotro model = new Hotro();
            model.Ticket = LoadTicketUser();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult yeucauhotro(FormCollection collection)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (!Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            Hotro model = new Hotro();
            model.Ticket = LoadTicketUser();
            var typeNV = collection["typeNV"];
            var Tittle = collection["TittleTicket"];
            var Send = collection["tText"];
            if (String.IsNullOrEmpty(Tittle))
            {
                ViewData["Err"] = "Vui lòng nhập tiêu đề";
                return View("yeucauhotro", model);
            }
            else if (String.IsNullOrEmpty(Send) || Equals(Send, "<p><br></p>"))
            {
                ViewData["Err"] = "Vui lòng nhập nội dung";
                return View("yeucauhotro", model);
            }
            else
            {
                try
                {
                    ticket tk = new ticket();
                    tickets_text tktext = new tickets_text();

                    tk.typeTicket = Int32.Parse(typeNV);
                    tk.DateCreate = DateTime.Parse(DateTime.Now.ToString());
                    tk.idtStatus = 6;
                    tk.TittleTicket = Tittle;
                    tk.idUserCreate = Int32.Parse(Session["dangnhap"].ToString());
                    tk.idUsetLastPost = Int32.Parse(Session["dangnhap"].ToString());
                    db.tickets.InsertOnSubmit(tk);
                    db.SubmitChanges();

                    tktext.idTicket = tk.idTicket;
                    tktext.idUser_post = Int32.Parse(Session["dangnhap"].ToString());
                    tktext.tText = Send;
                    tktext.DateSent = DateTime.Parse(DateTime.Now.ToString());
                    db.tickets_texts.InsertOnSubmit(tktext);
                    db.SubmitChanges();

                    ViewData["Success"] = "Tạo phiếu hỗ trợ thành công";
                    return RedirectToAction("chitietyeucauhotro", "support", new { id = tk.idTicket });
                }
                catch
                {
                    ViewData["Err"] = "Đã xảy ra lỗi vui lòng thử lại";
                    return View("yeucauhotro", model);
                }
            }
        }
        [HttpGet]
        public ActionResult dahoanthanh()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            var getid = Url.RequestContext.RouteData.Values["id"];
            if (getid == null) return RedirectToAction("Error404", "home");
            var a = from b in db.tickets
                    where Equals(b.idTicket, getid)
                    select b;
            foreach (var c in a)
            {
                c.idtStatus = 8;
                c.Closed = true;
            }
            db.SubmitChanges();
            return RedirectToAction("chitietyeucauhotro", "support", new { id = getid });
        }
        [HttpGet]
        public ActionResult dangxuly()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            if (Equals(Session["idLevel"], 0)) return RedirectToAction("Error404", "home");
            var getid = Url.RequestContext.RouteData.Values["id"];
            if (getid == null) return RedirectToAction("Error404", "home");
            var a = from b in db.tickets
                    where Equals(b.idTicket, getid)
                    select b;
            foreach (var c in a)
            {
                c.idtStatus = 7;
                c.Closed = false;
            }
            db.SubmitChanges();
            return RedirectToAction("chitietyeucauhotro", "support", new { id = getid });
        }
        [HttpGet]
        public ActionResult chitietyeucauhotro()
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            var getid = Url.RequestContext.RouteData.Values["id"];
            if (getid == null) return RedirectToAction("Error404", "home");
            Hotro mymodel = new Hotro();
            mymodel.Ticket = LoadChiTietTicketUser(getid.ToString());
            mymodel.Noidung = LoadNoiDung(getid.ToString());

            if (!Equals(Session["idLevel"], 0)) return View(mymodel);
            var a = (from b in db.tickets
                    where Equals(b.idUserCreate, Session["dangnhap"]) &&  Equals(b.idTicket, getid)
                    select b).Count();
            if(a != 0) return View(mymodel);
            return RedirectToAction("Error404", "home");
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult chitietyeucauhotro(FormCollection collection)
        {
            if (Session["dangnhap"] == null) return RedirectToAction("login", "home");
            var getid = Url.RequestContext.RouteData.Values["id"];
            if (getid == null) return RedirectToAction("Error404", "home");
            Hotro mymodel = new Hotro();
            mymodel.Ticket = LoadChiTietTicketUser(getid.ToString());
            mymodel.Noidung = LoadNoiDung(getid.ToString());
            var rep = collection["Reply"];
            if (String.IsNullOrEmpty(rep) || Equals(rep, "<p><br></pr>"))
            {
                ViewData["Err"] = "(*) Vui lòng nhập nội dung !";
                return View(mymodel);
            }
            else
            {
                tickets_text tx = new tickets_text();
                tx.idUser_post = Int32.Parse(Session["dangnhap"].ToString());
                tx.tText = rep;
                tx.idTicket = Int32.Parse(getid.ToString());
                tx.DateSent = DateTime.Parse(DateTime.Now.ToString());
                db.tickets_texts.InsertOnSubmit(tx);
                db.SubmitChanges();

                var a = from b in db.tickets
                        where Equals(b.idTicket, getid)
                        select b;
                foreach (var d in a)
                {
                    d.idUsetLastPost = Int32.Parse(Session["dangnhap"].ToString());
                }
                db.SubmitChanges();
                return RedirectToAction("chitietyeucauhotro", "support", new { id = getid });
            }
        }
        [NonAction]
        private List<Ticket> LoadChiTietTicketUser(string id)
        {
            List<Ticket> LoadChiTietTicketUser = new List<Ticket>();
            var query = from a in db.tickets
                        join c in db.logins on a.idUserCreate equals c.idUser
                        join b in db.accounts on a.idUsetLastPost equals b.idUser
                        join d in db.statusGenerals on a.idtStatus equals d.idStatus
                        where Equals(a.idTicket, id)
                        select new { a, b.Username, c.Email, d.nameStatus };
            foreach (var ta in query)
            {
                LoadChiTietTicketUser.Add(new Ticket()
                {
                    Email = ta.Email,
                    LastRep = ta.Username,
                    idTicket = ta.a.idTicket,
                    DateCreate = ta.a.DateCreate,
                    TittleTicket = ta.a.TittleTicket,
                    NameStatus = ta.nameStatus,
                    Closed = ta.a.Closed
                });
                break;
            }
            return LoadChiTietTicketUser;
        }
        [NonAction]
        private List<Ticket> LoadNoiDung(string id)
        {
            List<Ticket> LoadNoiDung = new List<Ticket>();
            var query = from b in db.tickets_texts
                        join c in db.accounts on b.idUser_post equals c.idUser
                        where Equals(b.idTicket, id)
                        orderby b.DateSent ascending
                        select new { b, c };
            foreach (var ta in query)
            {
                LoadNoiDung.Add(new Ticket()
                {
                    tText = ta.b.tText,
                    DateSent = ta.b.DateSent,
                    LastRep = ta.c.Username,
                    Avatar = ta.c.Image_Avatar
                });
            }
            return LoadNoiDung;
        }
        [NonAction]
        private List<Ticket> LoadTicketUser()
        {
            List<Ticket> LoadTicketUser = new List<Ticket>();
            var query = from a in db.tickets
                        join c in db.logins on a.idUserCreate equals c.idUser
                        join b in db.accounts on a.idUsetLastPost equals b.idUser
                        join d in db.statusGenerals on a.idtStatus equals d.idStatus
                        where Equals(a.idUserCreate, Session["dangnhap"])
                        select new { a, b.Username, c.Email, d.nameStatus };

            foreach (var ta in query)
            {
                LoadTicketUser.Add(new Ticket()
                {
                    idTicket = ta.a.idTicket,
                    Email = ta.Email,
                    LastRep = ta.Username,
                    DateCreate = ta.a.DateCreate,
                    TittleTicket = ta.a.TittleTicket,
                    NameStatus = ta.nameStatus
                });
            }
            return LoadTicketUser;
        }
        [NonAction]
        private List<Ticket> LoadTicketKinhDoanh()
        {
            List<Ticket> LoadTicketUser = new List<Ticket>();
            var query = from a in db.tickets
                        join c in db.logins on a.idUserCreate equals c.idUser
                        join b in db.accounts on a.idUsetLastPost equals b.idUser
                        join d in db.statusGenerals on a.idtStatus equals d.idStatus
                        where Equals(a.typeTicket, 1) && Equals(a.Closed, false)
                        select new { a, b.Username, c.Email, d.nameStatus };

            foreach (var ta in query)
            {
                LoadTicketUser.Add(new Ticket()
                {
                    idTicket = ta.a.idTicket,
                    Email = ta.Email,
                    LastRep = ta.Username,
                    DateCreate = ta.a.DateCreate,
                    TittleTicket = ta.a.TittleTicket,
                    NameStatus = ta.nameStatus
                });
            }
            return LoadTicketUser;
        }
        [NonAction]
        private List<Ticket> LoadTicketQuanLy()
        {
            List<Ticket> LoadTicketUser = new List<Ticket>();
            var query = from a in db.tickets
                        join c in db.logins on a.idUserCreate equals c.idUser
                        join b in db.accounts on a.idUsetLastPost equals b.idUser
                        join d in db.statusGenerals on a.idtStatus equals d.idStatus
                        where Equals(a.typeTicket, 2) && Equals(a.Closed, false)
                        select new { a, b.Username, c.Email, d.nameStatus };

            foreach (var ta in query)
            {
                LoadTicketUser.Add(new Ticket()
                {
                    idTicket = ta.a.idTicket,
                    Email = ta.Email,
                    LastRep = ta.Username,
                    DateCreate = ta.a.DateCreate,
                    TittleTicket = ta.a.TittleTicket,
                    NameStatus = ta.nameStatus
                });
            }
            return LoadTicketUser;
        }
        [NonAction]
        private List<Ticket> LoadTicketKinhDoanhHT()
        {
            List<Ticket> LoadTicketUser = new List<Ticket>();
            var query = from a in db.tickets
                        join c in db.logins on a.idUserCreate equals c.idUser
                        join b in db.accounts on a.idUsetLastPost equals b.idUser
                        join d in db.statusGenerals on a.idtStatus equals d.idStatus
                        where Equals(a.typeTicket, 1) && Equals(a.Closed, true)
                        select new { a, b.Username, c.Email, d.nameStatus };

            foreach (var ta in query)
            {
                LoadTicketUser.Add(new Ticket()
                {
                    idTicket = ta.a.idTicket,
                    Email = ta.Email,
                    LastRep = ta.Username,
                    DateCreate = ta.a.DateCreate,
                    TittleTicket = ta.a.TittleTicket,
                    NameStatus = ta.nameStatus
                });
            }
            return LoadTicketUser;
        }
        [NonAction]
        private List<Ticket> LoadTicketQuanLyHT()
        {
            List<Ticket> LoadTicketUser = new List<Ticket>();
            var query = from a in db.tickets
                        join c in db.logins on a.idUserCreate equals c.idUser
                        join b in db.accounts on a.idUsetLastPost equals b.idUser
                        join d in db.statusGenerals on a.idtStatus equals d.idStatus
                        where Equals(a.typeTicket, 2) && Equals(a.Closed, true)
                        select new { a, b.Username, c.Email, d.nameStatus };

            foreach (var ta in query)
            {
                LoadTicketUser.Add(new Ticket()
                {
                    idTicket = ta.a.idTicket,
                    Email = ta.Email,
                    LastRep = ta.Username,
                    DateCreate = ta.a.DateCreate,
                    TittleTicket = ta.a.TittleTicket,
                    NameStatus = ta.nameStatus
                });
            }
            return LoadTicketUser;
        }
    }
}