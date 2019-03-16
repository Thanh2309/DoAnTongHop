using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu_Hutech.Models;

namespace Edu_Hutech.Controllers
{
    public class AdminController : Controller
    {
        HutechEduDataContext db = new HutechEduDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["TaiKhoan_Admin"] == null)
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult DS_TaiKhoan(int ma_pc)
        {
            if (Session["TaiKhoan_Admin"] == null)
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            List<object> ds = new List<object>();
            ds.Add(db.TaiKhoan_ADMINs.Where(a => a.PhanCap == ma_pc).Select(a => a).OrderByDescending(a => a.TaiKhoan).ToList());
            ds.Add(db.Khoas.ToList());
            ds.Add(db.PhongBans.ToList());
            return PartialView(ds);
        }

        public JsonResult Xoa(string tai_khoan)
        {
            if (tai_khoan != null)
            {
                var tam1 = db.KQ_DanhGiaND_SV5T_Admins.Where(a => a.TaiKhoan == tai_khoan).FirstOrDefault();
                var tam2 = db.KQ_DanhGiaND_LHTT_Admins.Where(a => a.TaiKhoan == tai_khoan).FirstOrDefault();

                if (tai_khoan.EndsWith("01"))
                {
                    return Json(new { success = false, mess = "Đây là tài khoản cơ bản..., Không thể xóa !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (tam1 == null && tam2 == null)
                    {
                        db.Xoa_TaiKhoan_Admin(tai_khoan);
                        return Json(new { success = true, mess = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, mess = "Tài khoản có tài liệu liên quan, không thể xóa !" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { success = false, mess = "Không có tài khoản này... Thử lại" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChinhSua(string tai_khoan, string pass)
        {
            pass = pass.Trim();

            if (tai_khoan != null)
            {
                if (pass == "" || pass.Count() > 20)
                {
                    return Json(new { success = false, mess = "Vui lòng không để trống và đặt mật khẩu ít hơn 20 kí tự" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Sua_MatKhau_Admin(tai_khoan, pass);
                    return Json(new { success = true, mess = "Thay đổi thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, mess = "Có Lỗi !" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ThemMoi(string taikhoan, string pass, int ma_pc)
        {
            if (taikhoan != null)
            {
                var tam_tk = db.Lay_TK_Ad(taikhoan).FirstOrDefault().TaiKhoan;
                char[] tam = tam_tk.ToCharArray();
                int a = int.Parse(tam[tam.Count() - 2].ToString());
                int b = int.Parse(tam[tam.Count() - 1].ToString());
                if(b == 9)
                {
                    a = a + 1;
                    b = 0;
                }
                else
                {
                    b = b + 1;
                }

                string tk = taikhoan + a + b;
                db.Them_TaiKhoan_Admin(tk, pass, ma_pc);

                return Json(new { success = true, mess = "Thêm mới thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, mess = "Có Lỗi" }, JsonRequestBehavior.AllowGet);
        }
    }
}