using Edu_Hutech.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Edu_Hutech.Controllers
{
    public class NCKH_GVController : Controller
    {
        HutechEduDataContext db = new HutechEduDataContext();

        // GET: NCKH_GV
        public ActionResult Index()
        {
            string maGV = System.Web.HttpContext.Current.Session["TaiKhoan_GV"].ToString();
            var ds_PDK = db.DS_DeTai_NCKH_theoGV(maGV).ToList();
            return View(ds_PDK);
        }

        public ActionResult KiemDuyet(long maPDK)
        {
            ChiTietPDK_NCKH ct = new ChiTietPDK_NCKH();
            var chitiet = db.DS_SV_NCKH_theoMaPDK(maPDK).ToList();

            ct.MaPDK_NCKH = chitiet[0].MaPDK_NCKH;
            ct.ND_DeTai = "noidung_NCKH" + "_" + maPDK + ".doc";
            ct.SP_DeTai = "sanpham_NCKH" + "_" + maPDK + ".doc";
            ct.NgayDK = chitiet[0].NgayDK;
            ct.TenDeTai = chitiet[0].TenDeTai;
            ct.TinhTrang = chitiet[0].TinhTrang;

            ct.tenGV_1 = db.CanBo_HuongDans.Where(a => a.MaGV == chitiet[0].Ma_GVHD1).Select(a => a.TenGV).FirstOrDefault();

            ct.tenGV_2 = db.CanBo_HuongDans.Where(a => a.MaGV == chitiet[0].Ma_GVHD2).Select(a => a.TenGV).FirstOrDefault();

            ct.mssv_sv1 = chitiet[0].MSSV;
            ct.hoten_sv1 = chitiet[0].Ho + " " + chitiet[0].Ten;
            ct.malop_sv1 = chitiet[0].MaLop;
            ct.tenkhoa = chitiet[0].TenKhoa;

            if (chitiet.Count > 1)
            {
                ct.sv2 = true;
                ct.hoten_sv2 = chitiet[1].Ho + " " + chitiet[1].Ten;
                ct.mssv_sv2 = chitiet[1].MSSV;
                ct.malop_sv2 = chitiet[1].MaLop;
            }
            if (chitiet.Count > 2)
            {
                ct.sv3 = true;
                ct.hoten_sv3 = chitiet[2].Ho + " " + chitiet[2].Ten;
                ct.mssv_sv3 = chitiet[2].MSSV;
                ct.malop_sv3 = chitiet[2].MaLop;
            }
            if (chitiet.Count > 3)
            {
                ct.sv4 = true;
                ct.hoten_sv4 = chitiet[3].Ho + " " + chitiet[3].Ten;
                ct.mssv_sv4 = chitiet[3].MSSV;
                ct.malop_sv4 = chitiet[3].MaLop;
            }
            if (chitiet.Count > 4)
            {
                ct.sv5 = true;
                ct.hoten_sv5 = chitiet[4].Ho + " " + chitiet[4].Ten;
                ct.mssv_sv5 = chitiet[4].MSSV;
                ct.malop_sv5 = chitiet[4].MaLop;
            }

            return View(ct);
        }

        public ActionResult Luu()
        {
            long maPDK = long.Parse(System.Web.HttpContext.Current.Request.Params["maPDK"].ToString());
            string ten_dt = System.Web.HttpContext.Current.Request.Params["ten_dt"];

            var file_nd = Request.Files["file_nd"];

            var file_sp = Request.Files["file_sp"];

            string mssv = db.PhieuDangKy_DeTai_NCKHs.Where(s => s.MaPDK_NCKH == maPDK).Select(s => s.ND_DeTai).FirstOrDefault();
            string url = db.MinhChungs.Where(s => s.TenMInhChung == mssv).Select(s => s.URL).First().ToString();

            if (ten_dt == "")
            {
                return Json(new { success = false, mess = "Đề nghị nhập tên đề tài" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (file_nd == null || file_sp == null)
                {
                    db.PDK_NCKH_GV_Sua(maPDK, ten_dt);
                    return Json(new { success = true, mess = "Đã cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else if (file_nd.FileName == "" || file_sp.FileName == "")
                {
                    ViewBag.Thongbao = "Bổ sung tệp để hoàn thành đăng ký";
                }
                else
                {
                    db.PDK_NCKH_GV_Sua(maPDK, ten_dt);

                    var nd = Path.Combine(Server.MapPath(url), "noidung_NCKH" + "_" + maPDK + ".doc");
                    file_nd.SaveAs(nd);
                    var sp = Path.Combine(Server.MapPath(url), "sanpham_NCKH" + "_" + maPDK + ".doc");
                    file_sp.SaveAs(sp);

                    return Json(new { success = true, mess = "Đã cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, mess = "Có Lỗi" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Download(long? maPDK, string ten)
        {
            string mssv = db.PhieuDangKy_DeTai_NCKHs.Where(s => s.MaPDK_NCKH == maPDK).Select(s => s.ND_DeTai).FirstOrDefault();
            string url = db.MinhChungs.Where(s => s.TenMInhChung == mssv).Select(s => s.URL).First().ToString();
            var FileVirtualPath = url + "/" + ten + "_" + maPDK + ".doc";
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }

        public ActionResult ChinhSua()
        {
            long maPDK = long.Parse(System.Web.HttpContext.Current.Request.Params["maPDK"].ToString());
            if (maPDK != 0)
            {

                return Json(new { success = true, mess = "Đã duyệt thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, mess = "Có Lỗi" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DuyetPDK()
        {
            long maPDK = long.Parse(System.Web.HttpContext.Current.Request.Params["maPDK"].ToString());
            if (maPDK != 0)
            {
                db.PDK_NCKH_GV_duyet(maPDK);
                return Json(new { success = true, mess = "Đã duyệt thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, mess = "Có Lỗi" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HuyPDK()
        {
            long maPDK = long.Parse(System.Web.HttpContext.Current.Request.Params["maPDK"]);
            if (maPDK != 0)
            {
                db.PDK_NCKH_GV_huy(maPDK);
                return Json(new { success = true, mess = "Đã hủy thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, mess = "Có Lỗi" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelPDK()
        {
            long maPDK = long.Parse(System.Web.HttpContext.Current.Request.Params["maPDK"]);
            if (maPDK != 0)
            {
                db.Del_PDK_NCKH_theoMaPDK(maPDK);
                return Json(new { success = true, mess = "Đã thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, mess = "Có Lỗi" }, JsonRequestBehavior.AllowGet);
        }
    }
}