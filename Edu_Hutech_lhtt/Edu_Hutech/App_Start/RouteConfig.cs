using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Edu_Hutech
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap/{action}",
                defaults: new { controller = "UserLogin", action = "UserLogin", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "lhtt_user",
                url: "Lop-hoc-tien-tien/{action}",
                defaults: new { controller = "LopHocTienTien", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "sv5t_dangky_user",
                url: "sinh-vien-5-tot/dang-ky",
                defaults: new { controller = "SinhVien5Tot", action = "SV5T", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "sv5t_tiendo_user",
                url: "sinh-vien-5-tot/tien-do",
                defaults: new { controller = "SinhVien5Tot", action = "theodoi_SV5T", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "home",
                url: "trangchu",
                defaults: new { controller = "SinhVien5Tot", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "nckh_user",
                url: "nghien-cuu-khoa-hoc/{action}",
                defaults: new { controller = "NCKH", action = "NCKH", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "admin_khoa",
                url: "khoa",
                defaults: new { controller = "SV5T_Khoa_Phong", action = "TrangChu", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "admin_khoa_dk_sv5t",
                url: "khoa/danh-sach-dang-ky-sv5t",
                defaults: new { controller = "SV5T_Khoa_Phong", action = "DanhsachDK", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "admin_khoa_chitiet_sv5t",
                url: "khoa/chi-tiet-tieu-chi-sv5t",
                defaults: new { controller = "SV5T_Khoa_Phong", action = "danhgia_chitiet_sv", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "admin_khoa_dk_nckh",
                url: "khoa/danh-sach-dang-ky-nckh",
                defaults: new { controller = "SV5T_Khoa_Phong", action = "DanhsachNCKH", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "admin_khoa_chitiet_nckh",
                url: "khoa/chi-tiet-tieu-chi-nckh",
                defaults: new { controller = "SV5T_Khoa_Phong", action = "XetDuyet", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "admin_khoa_dk_lhtt",
                url: "khoa/danh-sach-dang-ky-lhtt",
                defaults: new { controller = "SV5T_Khoa_Phong", action = "LHTT_ThongKe", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "admin_khoa_chitiet_lhtt",
                url: "khoa/chi-tiet-tieu-chi-lhtt",
                defaults: new { controller = "SV5T_Khoa_Phong", action = "LHTT_Admin_DanhGia", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "admin_khoa_lop_truong",
                url: "khoa/quan-ly-lop-truong",
                defaults: new { controller = "SV5T_Khoa_Phong", action = "Loptruong", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "admin_gv",
                url: "giao-vien/{action}",
                defaults: new { controller = "NVKH_GV", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "admin_phong_ds_dk_sv5t",
                url: "phong/danh-sach-dang-ky-sv5t",
                defaults: new { controller = "Phong", action = "Danhsach_dk_sv5t_tr_th_tw", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "admin_phong_chitiet_sv5t",
                url: "phong/chi-tiet-tieu-chi-sv5t",
                defaults: new { controller = "Phong", action = "danhgia_chitiet_sv", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "admin_phong",
                url: "phong",
                defaults: new { controller = "Phong", action = "Phong", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "admin_truong",
                url: "truong/{action}",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "dang-nhap",
                url: "{controller}/{action}",
                defaults: new { controller = "UserLogin", action = "UserLogin", id = UrlParameter.Optional }
            );
        }
    }
}
