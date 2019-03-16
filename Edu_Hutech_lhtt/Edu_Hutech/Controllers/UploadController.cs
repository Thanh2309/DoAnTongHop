using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu_Hutech.Models;

namespace Edu_Hutech.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        HutechEduDataContext db = new HutechEduDataContext();
        [HttpPost]
        public ActionResult upLoadfile()
        {
            string mssv = System.Web.HttpContext.Current.Session["SinhVien"].ToString();
            var files = Request.Files;
            if (files.Count > 0)
            {
                    var file = files[0];
                    string url = db.MinhChungs.Where(s => s.TenMInhChung == mssv).Select(s => s.URL).First().ToString();
                    string namefile = System.Web.HttpContext.Current.Request.Params["name_file"];
                    var path = Path.Combine(Server.MapPath(url), namefile + ".jpg");
                    file.SaveAs(path);
                    long maPDK = db.PhieuDangKy_SV5Ts.Where(s => s.MSSV == mssv).Select(s => s.MaPDK_SV5T).SingleOrDefault();
                    db.Saukhi_upminhchung("" + maPDK, namefile);
                    db.SubmitChanges();

                    return Json(new { success = true, url_img = "/MinhChung/" + mssv + "/" + namefile + ".jpg" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "Có lỗi hoặc bạn chưa chọn hình ảnh!!" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult show_minhchung()
        {
           string mssv = System.Web.HttpContext.Current.Request.Params["maSSV"];
           string maND = System.Web.HttpContext.Current.Request.Params["maND_TC"];
           return Json(new { success = true, url_img = "/MinhChung/" + mssv + "/" + maND + ".jpg" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult upLoadfile_lhtt()
        {
            string mssv = System.Web.HttpContext.Current.Session["SinhVien"].ToString();
            var files = Request.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                string url = db.MinhChungs.Where(s => s.TenMInhChung == mssv).Select(s => s.URL).First().ToString();
                string namefile = System.Web.HttpContext.Current.Request.Params["name_file"];
                var path = Path.Combine(Server.MapPath(url), namefile + ".jpg");
                file.SaveAs(path);
                long maPDK = db.PhieuDangKy_LHTTs.OrderByDescending(x => x.MaPDK_LHTT).Where(s => s.MSSV == mssv).Select(s => s.MaPDK_LHTT).SingleOrDefault();
                db.Saukhi_upminhchung_LHTT("" + maPDK, namefile);
                return Json(new { success = true, url_img = "/MinhChung/" + mssv + "/" + namefile + ".jpg" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "Có lỗi hoặc bạn chưa chọn hình ảnh!!" }, JsonRequestBehavior.AllowGet);
        }
    }
}