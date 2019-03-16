using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu_Hutech.Models;
namespace Edu_Hutech.Controllers
{
    public class LopHocTienTienController : Controller
    {
        // GET: LopHocTienTien
        HutechEduDataContext db = new HutechEduDataContext();
        string mssv = System.Web.HttpContext.Current.Session["SinhVien"].ToString();
        public ActionResult DangKy()
        {
            var ho = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s).SingleOrDefault();
            string hoten = ho.Ho + " " + ho.Ten;
            ViewBag.HoTen = hoten;
            var kiemTraDangKy = db.PhieuDangKy_LHTTs.Where(s => s.MSSV == mssv).FirstOrDefault();
            if (kiemTraDangKy == null)
            {
                var maMC = db.MinhChungs.Where(a => a.TenMInhChung == mssv).Select(a => a.MaMinhChung).FirstOrDefault();
                string years = DateTime.Now.ToString().Trim();
                int year = Int32.Parse(years.Substring(6, 4));
                var tam = db.TieuChis.Where(b => b.MaPhanCap == "LHTT").ToList();
                int? maDK = db.DKy_LHTT(maMC, year, mssv).First().MaDK;
                foreach (var item in tam)
                {
                    db.DKy_LHTT_2(maDK, item.Ma_NDTieuChi);
                }
                return View();
            }
            else
                return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            var ho = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s).SingleOrDefault();
            string hoten = ho.Ho + " " + ho.Ten;
            ViewBag.HoTen = hoten;
            var view = db.GetTieuChiLHTT_TheoPDK(ho.PhieuDangKy_LHTTs.Select(c => c.MaPDK_LHTT).SingleOrDefault()).ToList();
            if(view.Count == 0)
            {
                ViewBag.ThongBao = "Chưa đăng ký Lớp học tiên tiến";
            }
            return View(view);
        }

    }
}