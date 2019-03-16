using Edu_Hutech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Edu_Hutech.Controllers
{
    public class PhongController : Controller
    {
        // GET: Phong
        HutechEduDataContext db = new HutechEduDataContext();
        public ActionResult Phong()
        {
            if (Session["TaiKhoan_Phong"] == null)
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ThoiGian_DangKy()
        {
            return PartialView();
        }
        public JsonResult Capnhat_Thoigiandangky(String maHD, DateTime ngayBD,DateTime ngayKT)
        {
            if (ngayBD.CompareTo(ngayKT) < 0 || ngayBD.CompareTo(ngayKT) == 0 )
            {
                db.ThoiGian_DangKy_PhongTrao(maHD, ngayBD, ngayKT);
                return Json(new { success = true, mess = "Thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, mess = "Ngày kết thúc phải sau ngày bắt đầu!" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Danhsach_dk_sv5t_tr_th_tw()
        {
            var ds = db.DSach_DK_SV5T_Cap_TR_TH_TW().ToList();
            return View(ds);
        }
        public ActionResult dsach_sv_dk_theo_khoa_cap(String maKhoa, String maCap)
        {   
            var ds = db.DSach_DK_SV5T_TheoKhoa_Cap(maKhoa, maCap,1,10).ToList();
            return PartialView(ds);
        }
        public ActionResult danhgia_chitiet_sv(long maPDK, string macap)
        {
            string tai_khoan = System.Web.HttpContext.Current.Session["TaiKhoan_Phong"].ToString() ;
            var list_tc = db.GetTaiLieu_SV5T_TheoCap(macap).ToList();
            // luu tiêu chí vào bảng dánh giá của admin với tình trạng là fasle
            var kt_maPDK_2 = db.KQ_DanhGiaND_SV5T_Admins.Where(s => s.MaPDK_SV5T == maPDK).Select(s => s.Dat).FirstOrDefault();
            if (kt_maPDK_2 == null)
            {
                foreach (var item in list_tc)
                {
                    db.Add_KQ_DanhGia_Sv5t_admin(tai_khoan, item.Ma_NDTieuChi, maPDK);
                }
            }
            //lay danh sach tieu chi theo maPDK( ten, ma, tinh trang minh chung, đạt hay chưa??)
            var ds = db.DSach_TC_Theo_MaPDK(maPDK).ToList();
            System.Web.HttpContext.Current.Session["maPDK"] = maPDK; // lưu lại để chỉnh sửa trạng thái ở KQ_DanhGiaND_SV5T_Admin
            return View(ds);
        }
        [HttpPost]
        public ActionResult QuanLyTieuChi(string ma_phancap)
        {
            if (Session["TaiKhoan_Phong"] == null)
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            List<object> list = new List<object>();

            ViewBag.bien = ma_phancap;
            var ds = db.TieuChis.Where(a => a.MaPhanCap == ma_phancap).Select(a => a).OrderByDescending(a => a.Ma_NDTieuChi).ToList();
            list.Add(db.TieuChuans.ToList());
            if(ds.Count == 0)
            {
                list.Add(db.TieuChis.Where(a => a.MaPhanCap != "LHTT").Select(a => a).OrderByDescending(a => a.Ma_NDTieuChi).ToList());
            }
            else
            {
                list.Add(db.TieuChis.Where(a => a.MaPhanCap == ma_phancap).Select(a => a).OrderByDescending(a => a.Ma_NDTieuChi).ToList());
            }
            return PartialView(list);
        }

        public ActionResult Create()
        {
            var ma_tchuan = System.Web.HttpContext.Current.Request.Params["ma_tieuchuan"];
            var ten_tieuchi = System.Web.HttpContext.Current.Request.Params["ten_tieuchi"];
            var diem = System.Web.HttpContext.Current.Request.Params["diem"];
            var khoa = Boolean.Parse(System.Web.HttpContext.Current.Request.Params["khoa"]);
            var truong = Boolean.Parse(System.Web.HttpContext.Current.Request.Params["truong"]);
            var thanh = Boolean.Parse(System.Web.HttpContext.Current.Request.Params["thanh"]);
            var trunguong = Boolean.Parse(System.Web.HttpContext.Current.Request.Params["trunguong"]);
            var batbuoc = Boolean.Parse(System.Web.HttpContext.Current.Request.Params["batbuoc"]);
            var ma_phancap = System.Web.HttpContext.Current.Request.Params["ma_phancap"];

            if(diem == null)
            {
                diem = "0";
            }

            List<TieuChi> tc_list = new List<TieuChi>();
            TieuChi tc = new TieuChi();

            if(ma_phancap == "LHTT")
            {
                var tieuchi = db.TieuChis.Where(a => a.MaPhanCap == "LHTT").OrderByDescending(a => a.Ma_NDTieuChi).FirstOrDefault().Ma_NDTieuChi;
                char[] tam = tieuchi.ToCharArray();
                int stt = int.Parse(tam[tam.Count() - 1].ToString()) + 1;

                tc.MaPhanCap = "LHTT";
                tc.Diem = float.Parse(diem);
                tc.MaTieuChi = ma_tchuan;
                tc.Ma_NDTieuChi = ma_tchuan + stt;
                tc.TenNDTieuChi = ten_tieuchi;
                tc.MaTieuChi = ma_tchuan;
                tc_list.Add(tc);

                db.TieuChis.InsertAllOnSubmit(tc_list);
                db.SubmitChanges();

                return Json(new { success = true, mess = "Thêm mới thành công!" }, JsonRequestBehavior.AllowGet);
            }
            else if(ma_phancap == "SV5T")
            {
                var tam = db.PhanCap_TieuChis.Where(a => (a.Khoa == khoa) && (a.Truong == truong) && (a.Thanh == thanh) && (a.TrungUong == trunguong)).Select(a => a.MaPhanCap).FirstOrDefault();
                if(tam.Count() == 0)
                {
                    khoa = true;
                }
                tc.MaPhanCap = db.PhanCap_TieuChis.Where(a => (a.Khoa == khoa) && (a.Truong == truong) && (a.Thanh == thanh) && (a.TrungUong == trunguong)).Select(a => a.MaPhanCap).FirstOrDefault();
                tc.Diem = float.Parse(diem);
                tc.MaTieuChi = ma_tchuan;

                var tieuchi = db.TieuChis.Where(a => a.MaTieuChi == ma_tchuan).OrderByDescending(a => a.Ma_NDTieuChi).FirstOrDefault().Ma_NDTieuChi;
                char[] abc = tieuchi.ToCharArray();
                int stt = int.Parse(abc[abc.Count() - 1].ToString()) + 1;

                tc.Ma_NDTieuChi = "SV" + ma_tchuan + stt;
                tc.TenNDTieuChi = ten_tieuchi;

                tc_list.Add(tc);

                db.TieuChis.InsertAllOnSubmit(tc_list);
                db.SubmitChanges();

                return Json(new { success = true, mess = "Thêm mới thành công!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, mess = "Thêm mới thành công!" }, JsonRequestBehavior.AllowGet);
        }
    }
}