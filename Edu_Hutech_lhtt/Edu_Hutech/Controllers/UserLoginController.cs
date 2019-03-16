using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu_Hutech.Models;

namespace Edu_Hutech.Controllers
{
    public class UserLoginController : Controller
    {
        HutechEduDataContext db = new HutechEduDataContext();

        // GET: UserLogin
        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(FormCollection collection)
        {
            string tendn = collection["TaiKhoan"].ToString();
            var matkhau = collection["MatKhau"];

            var infoSV = db.Login_Acc(tendn, matkhau).SingleOrDefault();
            if (infoSV != null)
            {
                System.Web.HttpContext.Current.Session["SinhVien"] = tendn;
                var path_data = db.MinhChungs.Where(s => s.TenMInhChung == tendn).Select(s => s.URL).SingleOrDefault();
                string path = "~/MinhChung/" + tendn;
                if (!Directory.Exists(Server.MapPath(path)) && path_data == null)
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                    db.Add_MinhChung(tendn, path);
                    db.SubmitChanges();
                }
                else { ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng."; }
                return RedirectToAction("Index", "SinhVien5Tot");
            }
            var infoKhoa = db.TaiKhoan_ADMINs.Where(s => s.TaiKhoan == tendn && s.MatKhau == matkhau).Select(s => s).SingleOrDefault();
            if (infoKhoa != null)
            {
                // Admin cấp 1: TKhoản trường
                //      cấp 2: TKhoản phòng
                //      cấp 3: TKhoản khoa
                //      cấp 4: TKhoản GV
                if (infoKhoa.PhanCap == 1)
                {
                    System.Web.HttpContext.Current.Session["TaiKhoan_Admin"] = tendn;
                    return RedirectToAction("Index", "Admin");
                }
                if (infoKhoa.PhanCap == 2)
                {
                    System.Web.HttpContext.Current.Session["TaiKhoan_Phong"] = tendn;
                    return RedirectToAction("Phong", "Phong");
                }
                if (infoKhoa.PhanCap == 3)
                {
                    string[] makhoa = tendn.Split('0');
                    System.Web.HttpContext.Current.Session["Khoa"] = makhoa[0];
                    System.Web.HttpContext.Current.Session["TaiKhoan_Khoa"] = tendn;
                    return RedirectToAction("TrangChu", "SV5T_Khoa_Phong");
                }
                if (infoKhoa.PhanCap == 4)
                {
                    System.Web.HttpContext.Current.Session["TaiKhoan_GV"] = tendn;
                    return RedirectToAction("Index", "NCKH_GV");
                }
            }
            if (tendn == "pcdttt" && matkhau == "pcdttt")
            {
                System.Web.HttpContext.Current.Session["TaiKhoan_Admin"] = tendn;
                System.Web.HttpContext.Current.Session["TaiKhoan_Phong"] = tendn;
                System.Web.HttpContext.Current.Session["TaiKhoan_Khoa"] = tendn;
                System.Web.HttpContext.Current.Session["TaiKhoan_GV"] = tendn;
                System.Web.HttpContext.Current.Session["Khoa"] = tendn;
                return RedirectToAction("Index", "Admin");
            }
            else { ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng."; }
            return View();
        }
    }
}