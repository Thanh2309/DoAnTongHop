using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu_Hutech.Models;

namespace Edu_Hutech.Controllers
{
    public class SinhVien5TotController : Controller
    {
        // GET: SinhVien5Tot
        HutechEduDataContext db = new HutechEduDataContext();

        static string mssv;
        
        public ActionResult Index()
        {
            
            if (Session["SinhVien"] == null)
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            mssv = System.Web.HttpContext.Current.Session["SinhVien"].ToString();
            var ho = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s).SingleOrDefault();
            string hoten = ho.Ho + " " + ho.Ten;
            ViewBag.HoTen = hoten;
            return View();
        }
        [HttpGet]
        public ActionResult Thongtin_sinhvien()
        {
            var ttsv = db.SinhViens.Where(s => s.MSSV == mssv).SingleOrDefault();
            return View(ttsv);
        }
        [HttpPost]
        public ActionResult Thongtin_sinhvien(FormCollection formCollection)
        {
            string sdt;
            if(formCollection["sdt"] != null)
            {
                sdt = formCollection["sdt"].ToString();
            }
            else
            {
                sdt = null;
            }
            string quequan;
            if (formCollection["quequan"] != null)
            {
                quequan = formCollection["quequan"].ToString();
            }
            else
            {
                quequan = null;
            }
            string email;
            if (formCollection["email"] != null)
            {
                email = formCollection["email"].ToString();
            }
            else
            {
                email = null;
            }
            string noisinh;
            if (formCollection["noisinh"] != null)
            {
                noisinh = formCollection["noisinh"].ToString();
            }
            else
            {
                noisinh = null;
            }
            string diachi_cutru;
            if (formCollection["diachi_cutru"] != null)
            {
                diachi_cutru = formCollection["diachi_cutru"].ToString();
            }
            else
            {
                diachi_cutru = null;
            }
            string diachi_lienlac;
            if (formCollection["diachi_lienlac"] != null)
            {
                diachi_lienlac = formCollection["diachi_lienlac"].ToString();
            }
            else
            {
                diachi_lienlac = null;
            }
            string ngay_vaodoan;
            if (formCollection["ngay_vaodoan"] != null)
            {
                ngay_vaodoan = formCollection["ngay_vaodoan"].ToString();
            }
            else
            {
                ngay_vaodoan = null;
            }
            string ngay_dang_dubi;
            if (formCollection["ngay_dang_dubi"] != null)
            {
                ngay_dang_dubi = formCollection["ngay_dang_dubi"].ToString();
            }
            else
            {
                ngay_dang_dubi = null;
            }
            string ngay_dang_chinhthuc;
            if (formCollection["ngay_dang_chinhthuc"] != null)
            {
                ngay_dang_chinhthuc = formCollection["ngay_dang_chinhthuc"].ToString();
            }
            else
            {
                ngay_dang_chinhthuc = null;
            }
            string chuc_vu_doan;
            if (formCollection["chuc_vu_doan"] != null)
            {
                chuc_vu_doan = formCollection["chuc_vu_doan"].ToString();
            }
            else
            {
                chuc_vu_doan = null;
            }
            db.Capnhat_TTSV(long.Parse(sdt), quequan, email, noisinh, diachi_cutru, diachi_lienlac, DateTime.Parse(ngay_vaodoan), DateTime.Parse(ngay_dang_dubi), DateTime.Parse(ngay_dang_chinhthuc), chuc_vu_doan, mssv);
            var ttsv = db.SinhViens.Where(s => s.MSSV == mssv).SingleOrDefault();
            return View(ttsv);
        }
        public ActionResult SV5T()
        {
            var ho = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s).SingleOrDefault();
            string hoten = ho.Ho + " " + ho.Ten;
            ViewBag.HoTen = hoten;
            var PDK = db.PhieuDangKy_SV5Ts.Where(s => s.MSSV == mssv).Select(s => s).FirstOrDefault();
            if (PDK == null)
            {
                return View();
            }
            string macap = PDK.MaCap;
            return RedirectToAction("theodoi_SV5T", new { macap = macap});
        }

        [HttpPost]
        public ActionResult Dky()
        {
            var maMC = db.MinhChungs.Where(a => a.TenMInhChung == mssv).Select(a => a.MaMinhChung).FirstOrDefault();
            string years = DateTime.Now.ToString().Trim();
            int year = Int32.Parse(years.Substring(6, 4));
            string macap = System.Web.HttpContext.Current.Request.Params["dky_sv5t"];            
            var tam = db.GetTaiLieu_SV5T_TheoCap(macap).ToList();
            int? maPDK = db.DKy_SV5T(maMC, macap, year, mssv).First().MaPDK;
            foreach (var item in tam)
                {
                    db.DKy_SV5T_2(maPDK,item.Ma_NDTieuChi);
                }
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult theodoi_SV5T(string macap)
        {
            var ho = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s).SingleOrDefault();
            string hoten = ho.Ho + " " + ho.Ten;
            ViewBag.HoTen = hoten;
            System.Web.HttpContext.Current.Session["MaCap"] = macap;
            var theo_doi = db.GetTieuChiSV5T_TheoMSSV_TheoCap(mssv,macap).ToList();
            return View(theo_doi);
        }
        public ActionResult LHTT()
        {
            var ho = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s).SingleOrDefault();
            string hoten = ho.Ho + " " + ho.Ten;
            ViewBag.HoTen = hoten;
            return View();
        }

        public void Report()
        {
            var pdk = db.PhieuDangKy_SV5Ts.Where(s => s.MSSV == mssv).Select(s => s).FirstOrDefault();
            var mc = db.CapDanhHieus.Where(s => s.MaCap == pdk.MaCap).Select(s => s).FirstOrDefault();
            var sv = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s).SingleOrDefault();
            var lop = db.Lops.Where(s => s.MaLop == sv.MaLop).Select(s => s).SingleOrDefault();
            var khoa = db.Khoas.Where(s => s.MaKhoa == lop.MaKhoa).Select(s => s).SingleOrDefault();
            
            string templatePath = Server.MapPath("~/Template/BieuMauSV5T.doc");
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();
            doc = app.Documents.Open(templatePath);
            doc.Activate();
            string gioitinh;
            if (sv.GioiTinh != null)
            {
                if (sv.GioiTinh == true)
                {
                    gioitinh = "Nam";
                }
                else { gioitinh = "Nữ"; }
            }
            else { gioitinh = "Không xác định"; }
            if (doc.Bookmarks.Exists("cap"))
            {
                doc.Bookmarks["cap"].Range.Text= mc.TenCap.ToUpper();
            }
            if (doc.Bookmarks.Exists("gioitinh"))
            {
                doc.Bookmarks["gioitinh"].Range.Text = gioitinh;
            }
            if (doc.Bookmarks.Exists("chucvu_Doan"))
            {
                doc.Bookmarks["chucvu_Doan"].Range.Text = sv.ChucVuDoan_neuco;
            }
            if (doc.Bookmarks.Exists("chuyennganh"))
            {
                doc.Bookmarks["chuyennganh"].Range.Text = "";
            }
            if (doc.Bookmarks.Exists("diachicutru"))
            {
                doc.Bookmarks["diachicutru"].Range.Text = sv.DiaChiCuTru;
            }
            if (doc.Bookmarks.Exists("diachilienlac"))
            {
                doc.Bookmarks["diachilienlac"].Range.Text = sv.DiaChiLienLac;
            }
            if (doc.Bookmarks.Exists("dienthoai"))
            {
                doc.Bookmarks["dienthoai"].Range.Text = sv.DiaChiLienLac;
            }
            if (doc.Bookmarks.Exists("email"))
            {
                doc.Bookmarks["email"].Range.Text = sv.Email;
            }
            if (doc.Bookmarks.Exists("hoten"))
            {
                doc.Bookmarks["hoten"].Range.Text = sv.Ho + " " + sv.Ten;
            }
            if (doc.Bookmarks.Exists("lop"))
            {
                doc.Bookmarks["lop"].Range.Text = lop.MaLop.ToString();
            }
            if (doc.Bookmarks.Exists("MSSV"))
            {
                doc.Bookmarks["MSSV"].Range.Text = sv.MSSV;
            }

            DateTime now = DateTime.Now;

            if (doc.Bookmarks.Exists("nam"))
            {
                doc.Bookmarks["nam"].Range.Text = now.Year.ToString();
            }

            int[] namhoc = Namhoc(now);
            if (doc.Bookmarks.Exists("namhoc"))
            {
                doc.Bookmarks["namhoc"].Range.Text = namhoc[0].ToString() + "-" + namhoc[1].ToString();
            }

            int namnhaphoc = int.Parse("20" + sv.MSSV.Substring(0, 2));
            if (doc.Bookmarks.Exists("namthu"))
            {
                doc.Bookmarks["namthu"].Range.Text = (Namhoc(now)[1] - namnhaphoc).ToString();
            }
            if (doc.Bookmarks.Exists("ngay"))
            {
                doc.Bookmarks["ngay"].Range.Text = now.Day.ToString();
            }
            if (doc.Bookmarks.Exists("thang"))
            {
                doc.Bookmarks["thang"].Range.Text = now.Month.ToString();
            }

            if (doc.Bookmarks.Exists("ngaysinh"))
            {
                doc.Bookmarks["ngaysinh"].Range.Text = sv.NgaySinh.ToLongDateString();
            }
            if (doc.Bookmarks.Exists("ngayvaoDang_ct"))
            {
                doc.Bookmarks["ngayvaoDang_ct"].Range.Text = sv.NgayVaoDang_chinhthuc_neuco.ToString();
            }
            if (doc.Bookmarks.Exists("ngayvaoDang_db"))
            {
                doc.Bookmarks["ngayvaoDang_db"].Range.Text = sv.NgayVaoDang_dubi_neuco.ToString();
            }
            if (doc.Bookmarks.Exists("ngayvaoDoan"))
            {
                doc.Bookmarks["ngayvaoDoan"].Range.Text = sv.NgayKetNapDoan.ToString();
            }
            if (doc.Bookmarks.Exists("noisinh"))
            {
                doc.Bookmarks["noisinh"].Range.Text = sv.NoiSinh;
            }
            if (doc.Bookmarks.Exists("quequan"))
            {
                doc.Bookmarks["quequan"].Range.Text = sv.QueQuan;
            }
            if (doc.Bookmarks.Exists("tenkhoa"))
            {
                doc.Bookmarks["tenkhoa"].Range.Text = khoa.TenKhoa;
            }
            if (doc.Bookmarks.Exists("tongnamhoc"))
            {
                doc.Bookmarks["tongnamhoc"].Range.Text = "4";
            }

            Response.AddHeader("Content-Disposition", "attachment; filename = MAU_KHAI_THANH_TICH_SV5T.doc");
            Response.Write(doc.Content.XML);

            doc.Saved = true;
            app.Application.Quit();
        }

        public int[] Namhoc(DateTime now)
        {
            int namhoc1;
            int namhoc2;
            if(now.Month >= 9)
            {
                namhoc1 = now.Year;
                namhoc2 = now.Year + 1;
            }
            else
            {
                namhoc1 = now.Year - 1;
                namhoc2 = now.Year;
            }
            int[] namhoc = { namhoc1, namhoc2 };

            return namhoc;
        }

        public JsonResult KiemTra_DKy()
        {
            var PDK_SV5T = db.PhieuDangKy_SV5Ts.Where(s => s.MSSV == mssv).Select(s => s).FirstOrDefault();
            var sv = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s).FirstOrDefault();
            var PDK_LHTT = db.PhieuDangKy_LHTTs.Where(s => s.MSSV == mssv).Select(s => s).FirstOrDefault();
            var tg_SV5T = db.ThoiGianHDs.Where(a => a.TenHD == "SV5T").Select(a => a).FirstOrDefault();
            var tg_LHTT = db.ThoiGianHDs.Where(a => a.TenHD == "LHTT").Select(a => a).FirstOrDefault();
            var tg_NCKH = db.ThoiGianHDs.Where(a => a.TenHD == "NCKH").Select(a => a).FirstOrDefault();
            DateTime now = DateTime.Now;

            bool dky_SV5T = true, dky_LHTT = true, dky_NCKH = true;
            string mess;
            string lt;

            if(tg_SV5T.TG_BatDau.Value.CompareTo(now) > 0 || tg_SV5T.TG_KetThuc.Value.CompareTo(now) < 0)
            {
                dky_SV5T = false;
            }
            if (tg_LHTT.TG_BatDau.Value.CompareTo(now) > 0 || tg_LHTT.TG_KetThuc.Value.CompareTo(now) < 0)
            {
                dky_LHTT = false;
            }
            if (tg_NCKH.TG_BatDau.Value.CompareTo(now) > 0 || tg_NCKH.TG_KetThuc.Value.CompareTo(now) < 0)
            {
                dky_NCKH = false;
            }
            if (sv.loptruong == false || sv.loptruong == null)
            {
                lt = "koloptruong";
            }
            else
            {
                lt = "loptruong";
            }
            if (PDK_SV5T != null && PDK_LHTT != null)
            {
                mess = "dadangky_LHTT_SV5T";
            }
            else
            {
                if (PDK_LHTT != null)
                {
                    mess = "dadangky_LHTT";
                }else if (PDK_SV5T != null)
                {
                    mess = "dadangky_SV5T";
                }
                else
                {
                    mess = "binhthuong";
                }
            }
            return Json(new { success = true, mess, lt, dky_SV5T, dky_LHTT, dky_NCKH }, JsonRequestBehavior.AllowGet);
        }
    }
}