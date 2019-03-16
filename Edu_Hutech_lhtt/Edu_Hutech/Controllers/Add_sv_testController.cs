using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu_Hutech.Models;
using PagedList;

namespace Edu_Hutech.Controllers
{
    public class Add_sv_testController : Controller
    {
        // GET: Add_sv_test
        HutechEduDataContext db = new HutechEduDataContext();
        public ActionResult Add_sv_test(int? Page_No)
        {
            //DateTime date = DateTime.Now;
            //for (int i = 1; i <= 500; i++)
            //{
            //    SinhVien sv = new SinhVien();
            //    sv.MSSV = "" + (1511065000 + i);
            //    sv.Ho = "Nguyễn";
            //    sv.Ten = "" + i;
            //    if (i <= 100)
            //    {
            //        sv.MaLop = "15DTH11";
            //        if (i == 15)
            //        {
            //            sv.loptruong = true;
            //        }
            //    }
            //    if (i > 100 && i <= 200)
            //    {
            //        sv.MaLop = "15DTH12";
            //        if (i == 150)
            //        {
            //            sv.loptruong = true;
            //        }
            //    }
            //    if (i > 200 && i <= 300)
            //    {
            //        sv.MaLop = "16DTHB4";
            //        if (i == 260)
            //        {
            //            sv.loptruong = true;
            //        }
            //    }
            //    if (i > 300 && i <= 400)
            //    {
            //        sv.MaLop = "17DTHC2";
            //        if (i == 315)
            //        {
            //            sv.loptruong = true;
            //        }
            //    }
            //    if (i > 400)
            //    {
            //        sv.MaLop = "18DTH02";
            //        if (i == 415)
            //        {
            //            sv.loptruong = true;
            //        }
            //    }
            //    Random random = new Random();
            //    if (random.Next(0, 1) == 0)
            //        sv.GioiTinh = true;
            //    else
            //        sv.GioiTinh = false;
            //    sv.password = "" + (1511065000 + i);
            //    sv.NoiSinh = "" + i;
            //    sv.QueQuan = "" + i + "" + i;
            //    sv.NgaySinh = date.Date;
            //    db.SinhViens.InsertOnSubmit(sv);
            //    db.SubmitChanges();
            //}
            //ViewBag.Xong = " Them Thanh Cong";
            var sv_list = db.SinhViens.ToList();
            int Size_Of_Page = 4;
            int No_Of_Page = (Page_No ?? 1);
            return PartialView(sv_list.ToPagedList(No_Of_Page, Size_Of_Page));
        }
    }
}