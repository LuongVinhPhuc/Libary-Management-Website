﻿using Microsoft.Ajax.Utilities;
using Quanlythuvien.Models;
using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Quanlythuvien.Controllers
{
    public class HomeController : Controller
    {
        Data db = new Data();
        public ActionResult Index()
        {
            if (Session["ID_TK"] != null)
            {
                ViewBag.TongSLS = SoLuongNhanVien();
                ViewBag.TongSLDG = SoLuongDocGia();
                ViewBag.SLSDM = SoLuongSachDamuon();
                ViewBag.SLSCM = SoLuongSach();
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //đăng nhập, đăng xuất
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string UserName, string PassWord)
        {
            if (ModelState.IsValid)
            {

                var user1 = db.NhanViens.FirstOrDefault(u => u.UserName.Equals(UserName) && u.PassWord.Equals(PassWord));
                if (user1 != null)
                {
                    var newCookie = new HttpCookie("myCookie", user1.MaNV.ToString());
                    newCookie.Expires = DateTime.Now.AddDays(10);
                    Response.AppendCookie(newCookie);
                    Session["HoTen"] = user1.HoTen;
                    Session["ID_TK"] = user1.MaNV;
                    Session["HinhAnh"] = user1.HinhAnh;
                    return Redirect("~/Home/Index");
                }

                else
                {
                    ViewBag.error = "Thông tin đăng nhập không hợp lệ !";
                }


            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            if (Request.Cookies["myCookie"] != null)
            {
                //Fetch the Cookie using its Key.
                HttpCookie nameCookie = Request.Cookies["myCookie"];

                //Set the Expiry date to past date.
                nameCookie.Expires = DateTime.Now.AddDays(-1);

                //Update the Cookie in Browser.
                Response.Cookies.Add(nameCookie);

                //Set Message in TempData.
                TempData["Message"] = "Cookie deleted.";
                Session["HoTen"] = "";
                Session["ID_TK"] = "";
                Session["HinhAnh"] = "";
            }
            return RedirectToAction("Login");
        }
        public ActionResult DanhMucSach(int? Id)
        {
            ViewData["tl"] = db.TheLoais.AsNoTracking().ToList();
            ViewData["kho"] = db.KhoSaches.AsNoTracking().ToList();
            var m = db.Saches.AsQueryable();
            if (Id != null)
            {
                var tt = Convert.ToInt16(Id);
                m = m.Where(g => g.TinhTrang == tt);
                var list1 = m.ToList();
                return View(list1);
            }
            else
            {
                var sx = Request["keysearch"];
                if (!string.IsNullOrEmpty(sx))
                {
                    m = m.Where(g => g.TacGia.Contains(sx) || g.MaKhoSach.Contains(sx) || g.NhaXB.Contains(sx));
                }
                var s = Request["sapxep"];
                if (s != null && s == "1")
                {
                    m = m.OrderBy(g => g.MaSach);
                }
                else if (s != null && s == "2")
                {
                    m = m.OrderByDescending(g => g.MaSach);
                }
                var list = m.ToList();
                ViewData["tl"] = db.TheLoais.AsQueryable().ToList();
                ViewData["kho"] = db.KhoSaches.AsQueryable().ToList();
                return View(list);
            }
        }

        [HttpPost]
        public ActionResult CapNhatTrangThai()
        {
            ViewData["tl"] = db.TheLoais.AsNoTracking().ToList();
            var ma = Request["ma"].ToString();
            var theloai = Request["TheLoai"].ToString();
            if (!string.IsNullOrEmpty(ma))
            {
                Sach m = db.Saches.FirstOrDefault(g => g.MaSach == ma);
                m.TinhTrang = Convert.ToInt16(Request["tinh-trang-sach"]);
                m.Id_TL = Convert.ToInt16(theloai);
                db.Saches.AddOrUpdate(m);
                db.SaveChanges();
                TempData["AlertMessage"] = "Danh mục sách đã được cập nhật !";

            }
            return Redirect("~/Home/DanhMucSach");

        }
        public ActionResult Create()
        {
            ViewData["kho"] = db.KhoSaches.AsNoTracking().ToList();
            ViewData["tl"] = db.TheLoais.AsNoTracking().ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Sach sach)
        {
            try
            {
                sach.MaSach = "";
                sach.MaKhoSach = Request["TenSach"];
                sach.TacGia = Request["TacGia"];
                sach.NhaXB = Request["NhaXB"];
                var m = Request["TheLoai"];
                sach.Id_TL = Convert.ToInt16(m);
                sach.TinhTrang = 1;
                db.Saches.Add(sach);
                db.SaveChanges();
                TempData["AlertMessage"] = "Danh mục sách đã được thêm mới !";
                return RedirectToAction("/DanhMucSach");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                return View(sach);

            }
        }
        public ActionResult ListDocGia()
        {
            ViewData["khoa"] = db.Khoas.AsNoTracking().ToList();
            var m = Request["key"];
            var list = db.DocGias.AsQueryable();
            if (!string.IsNullOrEmpty(m))
            {
                list = list.Where(g => g.MaDocGia.Contains(m) || g.TenDocGia.Contains(m) || g.MaSo.Contains(m));
            }
            return View(list.ToList());
        }
        public ActionResult CreateDG()
        {
            ViewData["khoa"] = db.Khoas.AsNoTracking().ToList();
            return View();
        }
        [HttpPost]
        public ActionResult CreateDG(DocGia dg)
        {
            try
            {
                dg.MaDocGia = "";
                dg.TenDocGia = Request["TenDocGia"];
                dg.MaSo = Request["maso"];
                var ns = Request["NgaySinh"];
                dg.NgaySinh = Convert.ToDateTime(ns);
                dg.GioiTinh = Request["GioiTinh"];
                dg.Khoa = Convert.ToInt16(Request["Khoa"]);
                db.DocGias.Add(dg);
                db.SaveChanges();
                TempData["AlertMessage"] = "Thêm độc giả thành công !";
                return RedirectToAction("/ListDocGia");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                return View(dg);

            }
        }

        public ActionResult QuanLiTheThanhVien(int? Id)
        {
            ViewData["hoivien"] = db.DocGias.AsNoTracking().ToList();
            var m = db.TheThanhViens.AsQueryable();
            ViewData["hoivien"] = db.DocGias.AsQueryable().ToList();
            if (Id != null)
            {
                var tt = Convert.ToInt16(Id);
                m = m.Where(g => g.TinhTrang == tt);
                var list = m.ToList();
                return View(list);
            }
            else
            {
                var k = Request["key"];
                if (!string.IsNullOrEmpty(k))
                {
                    m = m.Where(g => g.MaThe.Contains(k));
                }
                var list1 = m.ToList();
                return View(list1);

            }
        }


        public ActionResult MuonSach()
        {
            ViewData["dg"] = db.DocGias.AsNoTracking().ToList();
            ViewData["kho"] = db.KhoSaches.AsNoTracking().ToList();
            return View();
        }
        [HttpPost]
        public ActionResult MuonSach(MuonSach ms)
        {
            try
            {
                ms.MaPhieu = "";
                ms.MaSach = Request["MaSach"];
                ms.MaDocGia = Request["MaDocGia"];
                ms.NgayMuon = Convert.ToDateTime(Request["NgayMuon"]);
                ms.NgayHenTra = Convert.ToDateTime(Request["NgayHenTra"]);
                ms.NgayLapPhieu = Convert.ToDateTime(Request["NgayLapPhieu"]);
                ms.MaNV = Request.Cookies["myCookie"].Value;
                db.MuonSaches.Add(ms);
                db.SaveChanges();
                var m = db.KhoSaches.FirstOrDefault(g => g.MaSachKho == ms.MaSach);
                m.SoLuong--;
                db.KhoSaches.AddOrUpdate(m);
                db.SaveChanges();
                var n = db.Saches.FirstOrDefault(g => g.MaKhoSach == ms.MaSach);
                n.TinhTrang = 2;
                db.Saches.AddOrUpdate(n);
                db.SaveChanges();
                TempData["AlertMessage"] = "Thêm phiếu mượn thành công !";
                return RedirectToAction("/Quanlimuon");

            }
        
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "MuonSach"));
            }


        }
        public ActionResult Quanlimuon()
        {
            ViewData["hoivien"] = db.DocGias.AsNoTracking().ToList();
            ViewData["kho"] = db.KhoSaches.AsNoTracking().ToList();
            var sx = Request["keysearch"];
            var m = db.MuonSaches.AsQueryable();
            if (!string.IsNullOrEmpty(sx))
            {
                m = m.Where(g => g.MaPhieu.Contains(sx));
            }
            var s = Request["sapxep"];
            if (s != null && s == "1")
            {
                m = m.OrderBy(g => g.MaPhieu);
            }
            else if (s != null && s == "2")
            {
                m = m.OrderByDescending(g => g.MaPhieu);
            }
            var list = m.ToList();
            ViewData["hoivien"] = db.DocGias.AsQueryable().ToList();
            ViewData["kho"] = db.KhoSaches.AsQueryable().ToList();
            return View(list);
        }
        public ActionResult TraSach(string Id)
        {
            MuonSach m = db.MuonSaches.FirstOrDefault(g => g.MaPhieu == Id);
            return View(m);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TraSach(MuonSach ms)
        {
            MuonSach m = db.MuonSaches.FirstOrDefault(g => g.MaPhieu == ms.MaPhieu);
            db.MuonSaches.Remove(m);
            db.SaveChanges();
            var n = db.KhoSaches.FirstOrDefault(g => g.MaSachKho == ms.MaSach);
            n.SoLuong++;
            db.KhoSaches.AddOrUpdate(n);
            db.SaveChanges();
            var p = db.Saches.FirstOrDefault(g => g.MaKhoSach == ms.MaSach);
            p.TinhTrang = 1;
            db.Saches.AddOrUpdate(p);
            db.SaveChanges();
            TempData["AlertMessage"] = "Trả sách thành công !";
            return RedirectToAction("/Quanlimuon");
        }

        public ActionResult Quanlinv()
        {
            var k = Request["key"];
            var m = db.NhanViens.AsQueryable();
            if (!string.IsNullOrEmpty(k))
            {
                m = m.Where(g => g.MaNV.Contains(k) || g.HoTen.Contains(k));
            }
            var list = m.ToList();
            return View(list);
        }
        public ActionResult CreateThe()
        {
            ViewData["dg"] = db.DocGias.AsNoTracking().ToList();
            return View();
        }
        [HttpPost]
        public ActionResult CreateThe(TheThanhVien ttv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ttv.MaThe = "";
                    ttv.MaDocGia = Request["MaDocGia"];
                    ttv.NgayHetHan = Convert.ToDateTime(Request["NgayHetHan"]);
                    ttv.TinhTrang = 1;
                    db.TheThanhViens.AddOrUpdate(ttv);
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Tạo thẻ thành viên thành công !";
                    return RedirectToAction("/QuanLiTheThanhVien");

                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                return View(ttv);

            }
        }
        public ActionResult SuaThe(string Id)
        {
            ViewData["dg"] = db.DocGias.AsNoTracking().ToList();
            TheThanhVien m = db.TheThanhViens.FirstOrDefault(g => g.MaThe == Id);
            return View(m);
        }
        [HttpPost]
        public ActionResult SuaThe(TheThanhVien ttv)
        {
            try
            {

                ttv.MaDocGia = Request["MaDocGia"];
                ttv.NgayHetHan = Convert.ToDateTime(Request["NgayHetHan"]);
                db.TheThanhViens.AddOrUpdate(ttv);
                db.SaveChanges();
                TempData["AlertMessage"] = "Sửa thẻ thành viên thành công !";
                return RedirectToAction("/QuanLiTheThanhVien");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                return View(ttv);

            }
        }
        public ActionResult XoaThe(string Id)
        {
            TheThanhVien m = db.TheThanhViens.FirstOrDefault(g => g.MaThe == Id);
            return View(m);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaThe(TheThanhVien ttv)
        {
            TheThanhVien m = db.TheThanhViens.FirstOrDefault(g => g.MaThe == ttv.MaThe);
            db.TheThanhViens.Remove(m);
            db.SaveChanges();
            TempData["AlertMessage"] = "Xóa thẻ thành công !";
            return RedirectToAction("/QuanLiTheThanhVien");
        }
        public ActionResult CreateNV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNV(NhanVien nv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    nv.MaNV = "";
                    nv.HoTen = Request["HoTen"];
                    var ns = Request["NgaySinh"];
                    nv.NgaySinh = Convert.ToDateTime(ns);
                    nv.GioiTinh = Request["GioiTinh"];
                    nv.UserName = Request["user"];
                    nv.PassWord = Request["pass"];
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        //Use  Namespace  called  :	System.IO
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        //Lấy  tên  file  upload
                        string UploadPath = Server.MapPath("~/img/" + FileName);
                        //Copy  Và  lưu  file  vào  server.
                        f.SaveAs(UploadPath);
                        //Lưu  tên  file  vào  trường
                        nv.HinhAnh = FileName;
                    }
                    db.NhanViens.AddOrUpdate(nv);
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Thêm nhân viên thành công !";
                    return RedirectToAction("/Quanlinv");

                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                return View(nv);

            }
        }
        public ActionResult SuaNV(string Id)
        {
            NhanVien m = db.NhanViens.FirstOrDefault(g => g.MaNV == Id);
            return View(m);
        }
        [HttpPost]
        public ActionResult SuaNV(NhanVien nv)
        {
            try
            {
                nv.HoTen = Request["HoTen"];
                var ns = Request["NgaySinh"];
                nv.NgaySinh = Convert.ToDateTime(ns);
                nv.GioiTinh = Request["GioiTinh"];
                nv.UserName = Request["user"];
                nv.PassWord = Request["pass"];
                var f = Request.Files["ImageFile"];
                if (f != null && f.ContentLength > 0)
                {
                    //Use  Namespace  called  :	System.IO
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    //Lấy  tên  file  upload
                    string UploadPath = Server.MapPath("~/img/" + FileName);
                    //Copy  Và  lưu  file  vào  server.
                    f.SaveAs(UploadPath);
                    //Lưu  tên  file  vào  trường
                    nv.HinhAnh = FileName;
                }
                db.NhanViens.AddOrUpdate(nv);
                db.SaveChanges();
                TempData["AlertMessage"] = "Cập nhật nhân viên thành công !";
                Session["HoTen"] = nv.HoTen;
                Session["ID_TK"] = nv.MaNV;
                Session["HinhAnh"] = nv.HinhAnh;
                return RedirectToAction("/Quanlinv");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                return View(nv);

            }
        }
        public ActionResult XoaNV(string Id)
        {
            NhanVien m = db.NhanViens.FirstOrDefault(g => g.MaNV == Id);
            return View(m);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaNV(NhanVien nv)
        {
            NhanVien m = db.NhanViens.FirstOrDefault(g => g.MaNV == nv.MaNV);
            db.NhanViens.Remove(m);
            db.SaveChanges();
            TempData["AlertMessage"] = "Xóa nhân viên thành công !";
            return RedirectToAction("/Quanlinv");
        }
        public ActionResult SuaDG(string Id)
        {
            ViewData["khoa"] = db.Khoas.AsNoTracking().ToList();
            DocGia m = db.DocGias.FirstOrDefault(g => g.MaDocGia == Id);
            return View(m);
        }
        [HttpPost]
        public ActionResult SuaDG(DocGia dg)
        {
            try
            {
                dg.MaDocGia = Request["Ma"];
                dg.TenDocGia = Request["TenDocGia"];
                dg.MaSo = Request["maso"];
                var ns = Request["NgaySinh"];
                dg.NgaySinh = Convert.ToDateTime(ns);
                dg.GioiTinh = Request["GioiTinh"];
                dg.Khoa = Convert.ToInt16(Request["Khoa"]);
                db.DocGias.AddOrUpdate(dg);
                db.SaveChanges();
                TempData["AlertMessage"] = "Cập nhật độc giả thành công !";
                return RedirectToAction("/ListDocGia");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                return View(dg);

            }
        }

        public ActionResult XoaDG(string Id)
        {
            DocGia m = db.DocGias.FirstOrDefault(g => g.MaDocGia == Id);
            return View(m);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaDG(DocGia dg)
        {
            DocGia m = db.DocGias.FirstOrDefault(g => g.MaDocGia == dg.MaDocGia);
            db.DocGias.Remove(m);
            db.SaveChanges();
            TempData["AlertMessage"] = "Xóa độc giả thành công !";
            return RedirectToAction("/ListDocGia");
        }
        public int SoLuongSach()
        {
            int sach = db.Saches.Count();
            return sach;
        }
        public int SoLuongDocGia()
        {
            int dg = db.DocGias.Count();
            return dg;
        }
        public int SoLuongSachDamuon()
        {
            int sachdamuon = db.MuonSaches.Count();
            return sachdamuon;
        }
        public int SoLuongNhanVien()
        {
            int nhanvien = db.NhanViens.Count();
            return nhanvien;
        }
        public ActionResult SuaSach(string Id)
        {
            ViewData["kho"] = db.KhoSaches.AsNoTracking().ToList();
            Sach m = db.Saches.FirstOrDefault(g => g.MaSach == Id);
            return View(m);
        }
        [HttpPost]
        public ActionResult SuaSach(Sach s)
        {
            try
            {
                s.MaKhoSach = Request["TenSach"];
                s.TacGia = Request["TacGia"];
                s.NhaXB = Request["NhaXuatBan"];
                db.Saches.AddOrUpdate(s);
                db.SaveChanges();
                TempData["AlertMessage"] = "Sửa sách thành công !";
                return RedirectToAction("/DanhMucSach");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                return View(s);

            }

        }
        public ActionResult XoaSach(string Id)
        {
            Sach m = db.Saches.FirstOrDefault(g => g.MaSach == Id);
            return View(m);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaSach(Sach s)
        {
            Sach m = db.Saches.FirstOrDefault(g => g.MaSach == s.MaSach);
            db.Saches.Remove(m);
            db.SaveChanges();
            TempData["AlertMessage"] = "Xóa sách thành công !";
            return RedirectToAction("/DanhMucSach");
        }
        public ActionResult Quanlykho()
        {
            var k = Request["key"];
            var m = db.KhoSaches.AsQueryable();
            if (!string.IsNullOrEmpty(k))
            {
                m = m.Where(g => g.MaSachKho.Contains(k) || g.TenSach.Contains(k));
            }
            var list = m.ToList();
            return View(list);
        }

        public ActionResult CreateSachKho()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSachKho(KhoSach ks)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ks.MaSachKho = "";
                    ks.TenSach = Request["TenSach"];
                    ks.SoLuong = Convert.ToInt32(Request["SoLuong"]);
                    ks.NgayNhapKho = Convert.ToDateTime(Request["NgayNhapKho"]);
                    db.KhoSaches.AddOrUpdate(ks);
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Thêm sách vào kho thành công !";
                    return RedirectToAction("/Quanlykho");
                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                return View(ks);

            }
        }
        public ActionResult SuaSachKho(string Id)
        {
            KhoSach m = db.KhoSaches.FirstOrDefault(g => g.MaSachKho == Id);
            return View(m);
        }
        [HttpPost]
        public ActionResult SuaSachKho(KhoSach ks)
        {
            try
            {
                ks.MaSachKho = Request["Ma"];
                ks.TenSach = Request["TenSach"];
                ks.SoLuong = Convert.ToInt32(Request["SoLuong"]);
                ks.NgayNhapKho = Convert.ToDateTime(Request["NgayNhapKho"]);
                db.KhoSaches.AddOrUpdate(ks);
                db.SaveChanges();
                TempData["AlertMessage"] = "Sửa sách trong kho thành công !";
                return RedirectToAction("/Quanlykho");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                return View(ks);

            }
        }
        public ActionResult XoaSachKho(string Id)
        {
            KhoSach m = db.KhoSaches.FirstOrDefault(g => g.MaSachKho == Id);
            return View(m);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaSachKho(KhoSach ks)
        {
            KhoSach m = db.KhoSaches.FirstOrDefault(g => g.MaSachKho == ks.MaSachKho);
            db.KhoSaches.Remove(m);
            db.SaveChanges();
            TempData["AlertMessage"] = "Xóa sách trong kho thành công !";
            return RedirectToAction("/Quanlykho");
        }
    }
}