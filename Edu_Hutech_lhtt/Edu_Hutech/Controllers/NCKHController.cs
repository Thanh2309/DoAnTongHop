using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu_Hutech.Models;
namespace Edu_Hutech.Controllers
{
    public class NCKHController : Controller
    {
        // GET: NCKH
        HutechEduDataContext db = new HutechEduDataContext();
        static string mssv;
        [HttpGet]
        public ActionResult NCKH()
        {
            mssv = System.Web.HttpContext.Current.Session["SinhVien"].ToString();
            if (mssv == "")
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            var ho = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s).SingleOrDefault();
            string hoten = ho.Ho + " " + ho.Ten;
            ViewBag.HoTen = hoten;
            var ds_gvhd = db.CanBo_HuongDans.ToList();
            return View(ds_gvhd);
        }
        public ActionResult ten_sv()
        {
            mssv = System.Web.HttpContext.Current.Session["SinhVien"].ToString();
            if (mssv == "")
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            string ten = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s.Ten).First();
            string ho = db.SinhViens.Where(s => s.MSSV == mssv).Select(s => s.Ho).First();
            string hoten = ho + " " + ten;
            return Json(new { success = true, ten_sv = hoten, mssv = mssv }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult NCKH(FormCollection collection)
        {
            var tendt = collection["ten_detai"];

            var sv1 = collection["cb_sv1"];
            var ht1 = collection["ho_ten_1"];
            var mssv1 = collection["mssv_1"];

            var sv2 = collection["cb_sv2"];
            var ht2 = collection["ho_ten_2"];
            var mssv2 = collection["mssv_2"];

            var sv3 = collection["cb_sv3"];
            var ht3 = collection["ho_ten_3"];
            var mssv3 = collection["mssv_3"];

            var sv4 = collection["cb_sv4"];
            var ht4 = collection["ho_ten_4"];
            var mssv4 = collection["mssv_4"];

            var sv5 = collection["cb_sv5"];
            var ht5 = collection["ho_ten_5"];
            var mssv5 = collection["mssv_5"];

            var gvhd1 = collection["gvhd"];

            var file_nd = Request.Files["file_noidung"];

            var file_sp = Request.Files["file_sp_dukien"];

            DateTime ngayDKy = DateTime.Now;

            if (file_nd.FileName == "" || file_sp.FileName == "")
            {
                ViewBag.Thongbao = "Bổ sung tệp để hoàn thành đăng ký";
            }
            else
            {
                long? maPDK = db.DKy_NCKH(gvhd1, null, tendt, ngayDKy, mssv).Single().MaPDK;

                db.DKy_NCKH_2(mssv, maPDK);

                string url = db.MinhChungs.Where(s => s.TenMInhChung == mssv).Select(s => s.URL).First().ToString();

                var nd = Path.Combine(Server.MapPath(url), "noidung_NCKH" + "_" + maPDK + ".doc");
                file_nd.SaveAs(nd);

                var sp = Path.Combine(Server.MapPath(url), "sanpham_NCKH" + "_" + maPDK + ".doc");
                file_sp.SaveAs(sp);

                if (Check_SV_dky(mssv2, ht2, maPDK, sv2) == false)
                {
                    return this.NCKH();
                }
                if (Check_SV_dky(mssv3, ht3, maPDK, sv3) == false)
                {
                    return this.NCKH();
                }
                if (Check_SV_dky(mssv4, ht4, maPDK, sv4) == false)
                {
                    return this.NCKH();
                }
                if (Check_SV_dky(mssv5, ht5, maPDK, sv5) == false)
                {
                    return this.NCKH();
                }

                return RedirectToAction("Index", "SinhVien5Tot");
            }
            return this.NCKH();
        }

        protected bool Check_SV_dky(string mssv, string hoten, long? maPDK, string sv)
        {
            if (sv == "on")
            {
                if (!String.IsNullOrEmpty(hoten) && !String.IsNullOrEmpty(mssv))
                {
                    string ho = db.SinhViens.Where(a => a.MSSV == mssv).Select(a => a.Ho).FirstOrDefault();

                    if (ho != null)
                    {
                        db.DKy_NCKH_2(mssv, maPDK);
                        return true;
                    }
                    else
                    {
                        ViewBag.Thongbao = "Mã số sinh viên không tồn tại";
                        db.Del_PDK_NCKH_theoMaPDK(maPDK);
                        return false;
                    }
                }
                else
                {
                    ViewBag.Thongbao = "Hãy nhập thông tin sinh viên đăng ký";
                    db.Del_PDK_NCKH_theoMaPDK(maPDK);
                    return false;
                }
            }

            return true;
        }
    }
}