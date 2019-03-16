using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu_Hutech.Models;
namespace Edu_Hutech.Controllers
{
    public class SV5T_Khoa_PhongController : Controller
    {
        // GET: SV5T_Khoa_Phong
        HutechEduDataContext db = new HutechEduDataContext();
        static string makhoa;

        public ActionResult TrangChu()
        {
            if (Session["Khoa"] == null)
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            makhoa = System.Web.HttpContext.Current.Session["Khoa"].ToString();
            ThongKe_DK thongKe = new ThongKe_DK();
            thongKe.Sl_detai_nckh = long.Parse(db.SL_DK_NCKH_TheoKhoa(makhoa).First().tong.ToString());
            thongKe.Sl_dk_lhtt = long.Parse(db.SL_DK_LHTT_TheoKhoa(makhoa).First().tong.ToString());
            thongKe.Sl_sv5t_khoa = long.Parse(db.SoLuong_DK_SV5T_TheoKhoa("K", makhoa).First().Tong.ToString());
            thongKe.Sl_sv5t_truong = long.Parse(db.SoLuong_DK_SV5T_TheoKhoa("TR", makhoa).First().Tong.ToString());
            thongKe.Sl_sv5t_thanh_tw = long.Parse(db.SoLuong_DK_SV5T_TheoKhoa("TH", makhoa).First().Tong.ToString()) +long.Parse(db.SoLuong_DK_SV5T_TheoKhoa("TW", makhoa).First().Tong.ToString());
            thongKe.Tong_dk_sv5t = thongKe.Sl_sv5t_khoa + thongKe.Sl_sv5t_truong + thongKe.Sl_sv5t_thanh_tw;
            return View(thongKe);
        }
        public ActionResult DanhsachDK()
        {
            var lop = db.DSach_DK_SV5T_TheoKhoa(makhoa);
            return View(lop);
        }
        [HttpPost]
        public ActionResult ds_sv_dk(string ma_lop,string ma_cap)
        {
            System.Web.HttpContext.Current.Session["MaCap"] = ma_cap;
            var ds = db.DsachDK_SV5T_Lop(ma_lop, ma_cap).ToList();
                return PartialView(ds);
        }
        public ActionResult danhgia_chitiet_sv(long maPDK, string macap)
        {
            string tai_khoan = System.Web.HttpContext.Current.Session["TaiKhoan_Khoa"].ToString();
            var list_tc = db.GetTaiLieu_SV5T_TheoCap(macap).ToList();
            // luu tiêu chí vào bảng dánh giá của admin với tình trạng là fasle
            var kt_maPDK_2 = db.KQ_DanhGiaND_SV5T_Admins.Where(s => s.MaPDK_SV5T == maPDK).Select(s => s.Dat).FirstOrDefault();
            if ( kt_maPDK_2 == null)
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
        public JsonResult tieuchi_dat(string maND, string ghichu)
        {
            long maPDK = long.Parse(System.Web.HttpContext.Current.Session["maPDK"].ToString());
            if( maND != null)
            {
                db.Danh_Gia_TieuChi_Dat(maPDK, maND);
                db.GhiChu_MinhChung(ghichu, maPDK, maND);
                
                return Json(new { success = true, mess = "Đánh giá thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, mess = "Có Lỗi" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult tieuchi_chuadat(string maND, string ghichu)
        {
            long maPDK = long.Parse(System.Web.HttpContext.Current.Session["maPDK"].ToString());
            if (maND != null)
            {
                db.Danh_Gia_TieuChi_ChuaDat(maPDK, maND);
                db.GhiChu_MinhChung(ghichu, maPDK, maND);

                return Json(new { success = true, mess = "Đánh giá thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, mess = "Có Lỗi" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DanhsachNCKH()
        {
            makhoa = System.Web.HttpContext.Current.Session["Khoa"].ToString();
            var ds_data = db.DS_DeTai_NCKH_theoKhoa(makhoa).ToList();
            List<DanhsachNCKH_Khoa> ds = new List<DanhsachNCKH_Khoa>();

            foreach (var item in ds_data)
            {
                DanhsachNCKH_Khoa ctds = new DanhsachNCKH_Khoa();
                string Ten_GV1 = db.CanBo_HuongDans.Where(s => s.MaGV == item.Ma_GVHD1).Select(s => s.TenGV).FirstOrDefault();
                string Ten_GV2 = db.CanBo_HuongDans.Where(s => s.MaGV == item.Ma_GVHD2).Select(s => s.TenGV).FirstOrDefault();

                ctds.MaPDK_NCKH = item.MaPDK_NCKH;
                ctds.MSSV = item.MSSV;
                ctds.NgayDK = item.NgayDK;
                ctds.TenDeTai = item.TenDeTai;
                ctds.TinhTrang = item.TinhTrang;
                ctds.ten_gv1 = Ten_GV1;
                ctds.ten_gv2 = Ten_GV2;

                ds.Add(ctds);
            }

            return View(ds);
        }

        public ActionResult XetDuyet(long maPDK)
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

        public ActionResult DuyetPDK()
        {
            long maPDK = long.Parse(System.Web.HttpContext.Current.Request.Params["maPDK"].ToString());
            if (maPDK != 0)
            {
                db.PDK_NCKH_Khoa_duyet(maPDK);
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

        public ActionResult LHTT_ThongKe()
        {

            //string mssv = System.Web.HttpContext.Current.Session["SinhVien"].ToString();
            var a = db.DSach_DK_LHTT();
            return View(a);
        }

        public ActionResult LHTT_Admin_DanhGia(long maPDK)
        {
            string tai_khoan = System.Web.HttpContext.Current.Session["TaiKhoan_Khoa"].ToString();
            var a = db.GetTieuChiLHTT_TheoPDK(maPDK).ToList();
            // luu tiêu chí vào bảng dánh giá của admin với tình trạng là fasle
            var kt_maPDK_2 = db.KQ_DanhGiaND_LHTT_Admins.Where(s => s.MaPDK_LHTT == maPDK).Select(s => s.Diem).FirstOrDefault();
            if (kt_maPDK_2 == null)
            {
                foreach (var item in a)
                {
                    db.Add_KQ_DanhGia_LHTT_admin(tai_khoan, item.Ma_NDTieuChi, maPDK);
                }
            }
            //lay danh sach tieu chi theo maPDK( ten, ma, tinh trang minh chung, đạt hay chưa??)
            var ds = db.DSach_TC_Theo_MaPDK_LHTT(maPDK).ToList();
            System.Web.HttpContext.Current.Session["maPDK"] = maPDK; // lưu lại để chỉnh sửa trạng thái ở KQ_DanhGiaND_SV5T_Admin
            return View(ds);
        }
        public ActionResult tieuchi_dat_LHTT()
        {
            long maPDK = long.Parse(System.Web.HttpContext.Current.Session["maPDK"].ToString());
            string maND = System.Web.HttpContext.Current.Request.Params["maND_TC"];
            string ghichu = System.Web.HttpContext.Current.Request.Params["ghichu"];
            if (maND != null)
            {
                db.Danh_Gia_TieuChi_Dat_lhtt(maPDK, maND);
                db.GhiChu_MinhChung_LHTT(ghichu, maPDK, maND);
                return Json(new { success = true, mess = "Đánh giá thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, mess = "Có Lỗi" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult tieuchi_chuadat_LHTT()
        {
            long maPDK = long.Parse(System.Web.HttpContext.Current.Session["maPDK"].ToString());
            string maND = System.Web.HttpContext.Current.Request.Params["maND_TC"];
            string ghichu = System.Web.HttpContext.Current.Request.Params["ghichu"];
            if (maND != null)
            {
                db.Danh_Gia_TieuChi_ChuaDat_lhtt(maPDK, maND);
                db.GhiChu_MinhChung_LHTT(ghichu, maPDK, maND);
                return Json(new { success = true, mess = "Đánh giá thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, mess = "Có Lỗi" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Delete(long maPDK)
        {
            //note: DeleteAllOnSubmit not using DeleteOnSubmit -> vì sẽ bị sót dữ liệu do có nhiều dòng có cùng MaPDK nên sẽ bị lỗi
            var lop = db.KQ_danhgia_LHTT_SVs.Where(p => p.MaPDK_LHTT == maPDK);
            db.KQ_danhgia_LHTT_SVs.DeleteAllOnSubmit(lop);
            var lop2 = db.PhieuDangKy_LHTTs.Where(p => p.MaPDK_LHTT == maPDK);
            db.PhieuDangKy_LHTTs.DeleteAllOnSubmit(lop2);
            db.SubmitChanges();
            return Json(new { success = true, mess = "Đã xóa thành công" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Create(string mssv)
        {
            //Dùng InsertOnSubmit thêm view Create để chỉnh sửa thông tin
            var kiemtraLoptruong = db.SinhViens.Where(s => s.MSSV == mssv && s.loptruong == true).FirstOrDefault();
            var kiemTraDangKy = db.PhieuDangKy_LHTTs.Where(s => s.MSSV == mssv).FirstOrDefault();
            if (kiemtraLoptruong == null)
            {
                return Json(new { success = false, mess = "Đây không phải là lớp trưởng của lớp nên không thể thêm được" }, JsonRequestBehavior.AllowGet);
            }
            else if (kiemTraDangKy != null)
            {
                return Json(new { success = false, mess = "Lớp đã đăng ký lớp học tiên tiến" }, JsonRequestBehavior.AllowGet);
            }
            else
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
                return Json(new { success = true, mess = "Đã thêm lớp học tiên tiến thành công" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Loptruong()
        {
            List<DS_loptruong_theoKhoa> list_sv = new List<DS_loptruong_theoKhoa>();
            var ds_lop = db.Lops.Where(a => a.MaKhoa == makhoa).Select(a => a.MaLop).ToList();
            for(int i=0; i<ds_lop.Count(); i++)
            {
                DS_loptruong_theoKhoa sv = new DS_loptruong_theoKhoa();
                var tam = db.SinhViens.Where(a => (a.MaLop == ds_lop[i]) && (a.loptruong == true)).Select(a => a).ToList();
                if (tam.Count() == 0)
                {
                    sv.malop = ds_lop[i];
                    sv.mssv = "";
                    sv.hoten = "";
                }
                else {
                    sv.malop = tam[0].MaLop;
                    sv.mssv = tam[0].MSSV;
                    sv.hoten = tam[0].Ho + " " + tam[0].Ten;
                }
                list_sv.Add(sv);
            }
            
            return View(list_sv);
        }

        public JsonResult Save(string malop, string mssv)
        {
            var sv = db.SinhViens.Where(a => (a.MaLop == malop) && (a.MSSV == mssv)).Select(a => a.MSSV).FirstOrDefault();
            if (sv == null)
            {
                return Json(new { success = false, mess = "không có sinh viên này trong lớp" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var loptruong = db.SinhViens.Where(a => (a.MaLop == malop) && (a.loptruong == true)).Select(a => a.MSSV).FirstOrDefault();
                if (loptruong == null)
                {
                    db.QuanLyLopTruong_theoKhoa(mssv, true);
                }
                else
                {
                    db.QuanLyLopTruong_theoKhoa(loptruong, false);
                    db.QuanLyLopTruong_theoKhoa(mssv, true);
                }
            }

            return Json(new { success = true, mess = "Đánh giá thành công!" }, JsonRequestBehavior.AllowGet);
        }
    }
}